using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EasyJoystick;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashTimer = 0;
    private float dashCooldownTimer = 0;
    private bool isDashing = false;

    private Animator animator;
    private GameState gameState;

    public static PlayerController Instance;// Bien Tinh de sau nay goi dung cho de dang vi Player la suy nhat trong game
    private SpriteRenderer rdr;
    [SerializeField] private List<SkillCooldown> skillCooldowns;
    private GameManager gm => GameManager.Instance;
    private PlayerStat stat => PlayerStat.Instance;
    [SerializeField] private float attackRepeatCooldown;
    [SerializeField] private float splitSpacing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
    }
    void Update()
    {

        if (gm.gameState == GameState.GameOver)
        {
            GameManager.Instance.InitGameOver();
        }
        if (gm.gameState == GameState.Playing)
        {
            MoveControl();
            var enemy = EnemyManager.Instance.GetNearestEnemy(transform.position);
            if (enemy != null)
            {
                var origins = new List<Vector2>();
                var direction = (Vector2)(enemy.transform.position - transform.position).normalized;

                origins.Add((Vector2)transform.position);
                var splitCount = stat.AttackSplitCount;
                for (int i = 1; i < splitCount; i++)
                {
                    if (i % 2 != 0)
                    {
                        int splitIndex = Mathf.FloorToInt(-(i / 2)) - 1;
                        var spacing = splitIndex * splitSpacing;
                        var perpendicular = Vector2.Perpendicular(direction).normalized * spacing;
                        var origin = (Vector2)transform.position + perpendicular;
                        origins.Add(origin);
                    }
                    else
                    {
                        int splitIndex = Mathf.FloorToInt((i / 2));
                        var spacing = splitIndex * splitSpacing;
                        var perpendicular = Vector2.Perpendicular(direction).normalized * spacing;
                        var origin = (Vector2)transform.position + perpendicular;
                        origins.Add(origin);
                    }

                }

                foreach (var skillCooldown in skillCooldowns)
                {
                    skillCooldown.CooldownTimer += stat.IsAttackSpeedBuffed ? Time.deltaTime * 3 * stat.AttackRecoverySpeed
                                                                            : Time.deltaTime * stat.AttackRecoverySpeed;
                    if (skillCooldown.CooldownTimer >= skillCooldown.SkillInfor.Cooldown)
                    {
                        skillCooldown.CooldownTimer -= skillCooldown.SkillInfor.Cooldown;
                        foreach (var origin in origins)
                        {

                            for (int i = 0; i < stat.AttackRepeatCount; i++)
                            {
                                StartCoroutine(CastSkillDelay(attackRepeatCooldown * i, skillCooldown, direction, origin));
                            }
                        }
                    }
                }
            }
        }
    }
    IEnumerator CastSkillDelay(float delay, SkillCooldown skillCooldown, Vector2 direction, Vector2 skillOrigin)
    {
        yield return new WaitForSeconds(delay);
        var skill = Instantiate(skillCooldown.SkillInfor.SkillPrefab,
                         skillOrigin,
                          Quaternion.identity);
        skill.CastSkill(direction);
    }

    private void MoveControl()
    {

        var moveX = UIManager.Instance.joystick.Horizontal();
        var moveY = UIManager.Instance.joystick.Vertical();
        // var moveX = Input.GetAxis("Horizontal");
        //  var moveY = Input.GetAxis("Vertical");
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            animator.SetBool("Running", true);
            animator.SetBool("Hit", false);


        }
        else if (moveX > 0 || moveY > 0 || moveY < 0)
        {
            transform.localScale = Vector3.one;
            animator.SetBool("Running", true);
            animator.SetBool("Hit", false);

        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("Hit", false);

        }

        transform.position += new Vector3(moveX, moveY) * stat.Speed * Time.deltaTime;

        dashCooldownTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        if (dashTimer > dashDuration)
        {
            isDashing = false;
            rb.velocity = Vector2.zero;
        }
        if (moveX != 0 || moveY != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer >= dashCooldown)
            {
                isDashing = true;
                dashTimer = 0;
                dashCooldownTimer = 0;
                var dashDirection = new Vector2(moveX, moveY).normalized;
                rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
            }
        }
    }

}
[System.Serializable]
public class SkillCooldown
{
    [SerializeField] SkillInfor skillInfor;
    public float CooldownTimer;

    public SkillInfor SkillInfor => skillInfor;

}
public enum GameState
{
    Playing,
    GameOver,
}


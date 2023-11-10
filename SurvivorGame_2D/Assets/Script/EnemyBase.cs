using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract partial class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float maxHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackCoolDown; // hoi chieu tan cong
    [SerializeField] protected float attackDamage; // Gay bao nhieu damage
    [SerializeField] protected float attackRange;//Tam Danh
    private SpriteRenderer rdr;
    public float MoveSpeed => moveSpeed;
    protected Rigidbody2D rb;
    protected float currentHP;
    protected float attacktimer = 0;

    public HealthEmenyBar healthEmenyBar;
    private float[] randomItemWeight = new float[] { 30, 30, 40, 300 };
    [SerializeField] private List<GameObject> itemDrops;

     public AudioSource hitAudioSr;

    private void Awake()
    {
        attacktimer = 0; // doi 1 luc moi tan cong neu no bang 0
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        rdr = GetComponent<SpriteRenderer>();
        healthEmenyBar.SetMaxHealth(maxHP);

    }
    private void Update()
    {
        var dt = Time.deltaTime;
        attacktimer += dt;

        AIControl(dt);
    }

    protected abstract void AIControl(float dt);

    public void TakeDamage(float damage)
    {
        hitAudioSr.Play();
        currentHP -= damage;
        healthEmenyBar.SetHealth(currentHP);
        rdr.DOFade(0f, 0.1f).SetLoops(6, LoopType.Yoyo).SetEase(Ease.InBounce).OnComplete(() => rdr.DOFade(1f, 0.1f));
        if (currentHP <= 0)
        {
            //Ke dich chet
            OnDead();
        }

    }
    private void OnDead()
    {
        EnemyManager.Instance.RemoveEnemyFromList(this);
        var index = RandomItem();
        SpawnItem(index);

        PlayerStat.Instance.AddEXP(30);
        UIManager.Instance.enemyDeadCount +=1;
        Destroy(gameObject);
    }
    private void SpawnItem(int index)
    {
        if (randomItemWeight.Count() - 1 <= index) return;
        Instantiate(itemDrops[index], transform.position, Quaternion.identity);
    }
    private int RandomItem()
    {
        float weightedSum = randomItemWeight.Sum();
        float randomRoll = Random.Range(0, weightedSum);
        float currentSum = 0;
        for (int i = 0; i < randomItemWeight.Count(); i++)
        {
            currentSum += randomItemWeight[i];
            if (currentSum > randomRoll)
            {
                return i;
            }
        }
        return 0;
    }


}

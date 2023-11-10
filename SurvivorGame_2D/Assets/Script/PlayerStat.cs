using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PlayerStat : MonoBehaviour
{
    private Animator animator;
    public AudioSource hitAudioSr;
    public AudioSource deadAudioSr;


    public static string SpeedId = "speed";
    public static string DamageId = "damage";
    public static string AttackRecoverySpeedId = "attack_speed";
    public static string AttackRepeatId = "attack_repeat";

    public static string AttackSplitId = "attack_split_count";
    public static float LevelThreshold = 100f;

    public static PlayerStat Instance;

    [SerializeField] private float baseMaxHp;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseAttackRecoverySpeed;

    private int attackSplitCount;
    private int attackRepeatCount;
    private float attackRecoverySpeed;
    private float speed;
    private float exp;
    private float level;

    public int AttackSplitCount => attackSplitCount;
    public float AttackRecoverySpeed => attackRecoverySpeed;
    public float Speed => IsSpeedBuffed ? baseSpeed * 1.5f : baseSpeed;
    public bool IsAttackSpeedBuffed { get; private set; } = false;
    public bool IsSpeedBuffed { get; private set; } = false;
    public int AttackRepeatCount => attackRepeatCount;

    private float currentHP;
    private float attackSpeedBuffTimer = 0f;
    private float speedBuffTimer = 0f;
    private List<ItemInfoBase> itemInfos = new List<ItemInfoBase>();

    public SpriteRenderer rdr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rdr = GetComponent<SpriteRenderer>();
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

    private void Start()
    {
        InitStat();
    }

    private void InitStat()
    {
        currentHP = baseMaxHp;
        speed = baseSpeed;
        exp = 0;
        level = 1;
        attackSplitCount = 1;
        attackRepeatCount = 1;
        attackRecoverySpeed = baseAttackRecoverySpeed;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            attackSpeedBuffTimer -= Time.deltaTime;
            speedBuffTimer -= Time.deltaTime;
            if (attackSpeedBuffTimer <= 0)
            {
                IsAttackSpeedBuffed = false;
            }
            if (speedBuffTimer <= 0)
            {
                IsSpeedBuffed = false;
            }
        }
    }

    public void AddItem(ItemInfoBase itemInfo)
    {
        itemInfos.Add(itemInfo);
        UpdateStats();
    }

    public void UpdateStats()
    {
        speed = GetUpgradedValue(baseSpeed, SpeedId);
        attackRecoverySpeed = GetUpgradedValue(baseAttackRecoverySpeed, AttackRecoverySpeedId);
        attackRepeatCount = Mathf.FloorToInt(GetUpgradedValue(1, AttackRepeatId));
        attackSplitCount = Mathf.FloorToInt(GetUpgradedValue(1, AttackSplitId));
    }

    public float GetUpgradedValue(float baseValue, string statId)
    {
        var flatValue = itemInfos.Where(item => item.StatId.Equals(statId) && item.ValueType == UpgradeValueType.Flat)
        .Select(item => item.Value).Sum();
        baseValue += flatValue;
        var percentageValue = itemInfos.Where(item => item.StatId.Equals(statId) && item.ValueType == UpgradeValueType.Percentage)
        .Select(item => item.Value).Sum();
        baseValue += baseValue * percentageValue / 100f;
        return baseValue;
    }

    public void AddEXP(float expValue)
    {
        exp += expValue;
        if (exp >= 100)
        {
            exp -= 100;
            level++;
            UIManager.Instance.levelCount += 1;
            UIManager.Instance.OpenUpgradePopup();
        }
        UIManager.Instance.UpdateXP(exp / 100f);
    }

    public void HealPlayer(float healAmount)
    {
        currentHP += healAmount;
        if (currentHP > baseMaxHp) currentHP = baseMaxHp;
        HealthFollow.Instance.UpdateHP(currentHP / baseMaxHp);
    }
    public void TakeDamage(float damage)
    {
        hitAudioSr.Play();
        currentHP -= damage;
        HealthFollow.Instance.UpdateHP(currentHP / baseMaxHp);
        rdr.DOFade(0f, 0.1f).SetLoops(6, LoopType.Yoyo).SetEase(Ease.InBounce).OnComplete(() => rdr.DOFade(1f, 0.1f));
        animator.SetBool("Hit",true);
        if (currentHP <= 0)
        {
            // animator.SetBool("Dead",true);
            deadAudioSr.Play();
            GameManager.Instance.InitGameOver();
        }
    }

    public void BoostMoveSpeed(float duration)
    {
        IsSpeedBuffed = true;
        speedBuffTimer = 60f;
    }

    public void BoostAttackSpeed(float duration)
    {
        IsAttackSpeedBuffed = true;
        attackSpeedBuffTimer = 60f;
    }
}
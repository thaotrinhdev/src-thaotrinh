using UnityEditor.Callbacks;
using UnityEngine;

public abstract class SkillInfor : ScriptableObject
{
    [SerializeField] protected float cooldown;
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private SkillBase skillPrefab;
    public float Cooldown => cooldown; // Ben ngoai chi dc lay chu khong duoc sua
    public float Damage => damage;
    public SkillBase SkillPrefab => skillPrefab;
    public float ProjectileSpeed => projectileSpeed;
}


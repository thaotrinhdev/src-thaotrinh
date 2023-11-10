using UnityEngine;

public class EnemyBasicSkill: SkillBase
{
    public override void CastSkill(Vector2 direction)
    {
        rb.velocity = direction*skillInfor.ProjectileSpeed; // Ban theo huong nhat dinh
        StartCoroutine(SelfDestroy()); // Funtion de pause trong 1 thoi gian nhat dinh
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.gameObject.tag == "Player")
        {
           
            PlayerStat.Instance.TakeDamage(skillInfor.Damage);
            Destroy(gameObject);

        }
    }
}

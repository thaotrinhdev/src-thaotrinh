using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESkills : SkillBase
{
    public GameObject Blood;
    public AoeSkillInfor aoeSkillInfor => skillInfor as AoeSkillInfor; // Bien skillInfor thanh AoeSkillInfor
    public override void CastSkill(Vector2 direction)
    {
        rb.velocity = direction * skillInfor.ProjectileSpeed; // Ban theo huong nhat dinh
        StartCoroutine(SelfDestroy()); // Funtion de pause trong 1 thoi gian nhat dinh
    }
    //CircleCast: tao vung lan toa khi co vu no
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            RaycastHit2D[] enemisHits = Physics2D.CircleCastAll(transform.position, aoeSkillInfor.AoeRange, Vector2.zero, 0f, layerMask: 1 << 8);
            foreach (var enemy in enemisHits)
            {
                var enemyBase = enemy.collider.GetComponent<EnemyBase>();
                enemyBase.TakeDamage(skillInfor.Damage);
            }
            Instantiate(Blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }
}

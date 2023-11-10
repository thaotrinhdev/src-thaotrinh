using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicSkill : SkillBase
{
    public override void CastSkill(Vector2 direction)
    {
        rb.velocity = direction*skillInfor.ProjectileSpeed; // Ban theo huong nhat dinh
        StartCoroutine(SelfDestroy()); // Funtion de pause trong 1 thoi gian nhat dinh
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.gameObject.tag == "Enemy")
        {
            var enemy = other.GetComponent<EnemyBase>();
            enemy.TakeDamage(skillInfor.Damage);
            Destroy(gameObject);

        }
    }
}

using UnityEngine;

public class BasicEnemy : EnemyBase
{
    protected override void AIControl(float dt)
    {
        var playerPos = PlayerController.Instance.transform.position;
        var moveDirection = playerPos - transform.position;
        var distance = moveDirection.sqrMagnitude;
        if (distance > 0.1f)
        {
            transform.position += moveDirection.normalized * moveSpeed * dt;
        }
        if (distance <= attackRange)
        {
            if (attacktimer >= attackCoolDown)
            {
                attacktimer = 0;
                PlayerStat.Instance.TakeDamage(attackDamage);
            }

        }
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
}

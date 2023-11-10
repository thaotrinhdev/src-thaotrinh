using Unity.Mathematics;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [SerializeField] private float agroRange;
    [SerializeField] private SkillBase bulletPrefab;
    protected override void AIControl(float dt)
    {
        var playerPos = PlayerController.Instance.transform.position;
        var moveDirection = playerPos - transform.position;
        var distance = moveDirection.sqrMagnitude;
        if (distance <= agroRange)
        {
            //Trang thai gan nguoi choi
            if (attacktimer >= attackCoolDown)
            {
                attacktimer = attackCoolDown - attacktimer; // reset time
                ShootBullet(moveDirection.normalized); // normalized : Chuyen thanh vecter don vi
            }
        }
        else
            //Follow ng choi
            transform.position += moveDirection.normalized * moveSpeed * dt;
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    private void ShootBullet(Vector2 direction)
    {
        var skill = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        skill.CastSkill(direction);
    }
}

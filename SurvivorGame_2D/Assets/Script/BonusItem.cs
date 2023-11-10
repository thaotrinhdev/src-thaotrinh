using UnityEngine;

public class BonusItem : MonoBehaviour
{
    [SerializeField] private float hpHealthAoumt;
    [SerializeField] private bool isSpeedBoost;
    [SerializeField] private float speedBoostAmount;
    [SerializeField] private bool isAttackSpeedBoost;
    [SerializeField] private float attackSpeedBoostAmount;
    [SerializeField] private float duantion;
     private PlayerStat pStat => PlayerStat.Instance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isSpeedBoost) pStat.BoostMoveSpeed(60f);
            if (isAttackSpeedBoost) pStat.BoostAttackSpeed(60f);
            pStat.HealPlayer(15f);
            Destroy(gameObject);
        }
    }

}
public enum UpgradeValueType
{
    Percentage,
    Flat
}

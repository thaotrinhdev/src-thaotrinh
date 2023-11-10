using System.Collections;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected PlayerController pc => PlayerController.Instance;
    [SerializeField] protected SkillInfor skillInfor;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    protected IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public abstract void CastSkill(Vector2 direction);

}


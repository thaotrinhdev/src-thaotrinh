using UnityEngine;

[CreateAssetMenu(menuName = "SkillInfor/AoeSkill")]
public class AoeSkillInfor : SkillInfor

{
    [SerializeField]private float aoeRange;
    public float AoeRange => aoeRange;
}


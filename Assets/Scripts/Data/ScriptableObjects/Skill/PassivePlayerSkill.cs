using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkill", menuName = "Player/Skill/Passive")]
public class PassivePlayerSkill : PlayerSkill
{
    private void OnEnable() => Type = SkillType.Passive;
}
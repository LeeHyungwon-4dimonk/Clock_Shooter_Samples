using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkill", menuName = "Player/Skill/Active")]
public class ActivePlayerSkill : PlayerSkill
{
    public int CooldownTurn;

    private void OnEnable() => Type = SkillType.Active;
}
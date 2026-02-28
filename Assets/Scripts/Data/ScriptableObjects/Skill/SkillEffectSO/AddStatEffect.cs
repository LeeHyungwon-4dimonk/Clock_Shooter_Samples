using UnityEngine;

[CreateAssetMenu(menuName = "SkillEffect/Stat/Add")]
public class AddStatEffect : PlayerSkillEffectSO
{
    public StatType statType;
    public StatModifierType modifierType;
    public float value;
    public float durationTurn = -1;

    public override void Apply(SkillContext context)
    {
        context.Stats.AddStat(statType, value, modifierType, durationTurn);
    }
}
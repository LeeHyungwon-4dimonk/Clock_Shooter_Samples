using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillEffectSO : ScriptableObject
{
    public abstract void Apply(SkillContext context);

    public virtual IEnumerable<StatModifierDescriptor> GetStatModifiers()
    {
        yield break;
    }
}

public class SkillContext
{
    public PlayerStats Stats;
    public TurnSystem Turn;
    public ProjectileSystem Projectile;
}

public struct StatModifierDescriptor
{
    public StatType StatType;
    public StatModifierType ModifierType;
    public float Value;
    public float Duration;
}
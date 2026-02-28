using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum SkillType
{
    Passive,
    Active,
}

public abstract class PlayerSkill : ScriptableObject
{
    [SerializeField] private string _skillID;
    public string SkillID => _skillID;

    public LocalizedString SkillName;
    public LocalizedString SkillDescription;
    public SkillType Type;
    public Sprite Icon;
    public bool AllowDuplicate = true;

    public List<PlayerSkillEffectSO> Effects;

    public void Apply(SkillContext context)
    {
        foreach (var effect in Effects)
            effect.Apply(context);
    }
}

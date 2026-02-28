using System.Collections.Generic;

public class Stat
{
    public float BaseValue;
    private List<StatModifier> _modifiers = new();

    public float Value
    {
        get
        {
            float flat = 0f;
            float percent = 0f;

            foreach (var mod in _modifiers)
            {
                if (mod.Type == StatModifierType.Flat)
                    flat += mod.Value;
                else
                    percent += mod.Value;
            }
            return (BaseValue + flat) * (1 + percent);
        }
    }

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }

    public List<StatModifier> GetModifiers() => _modifiers;
}
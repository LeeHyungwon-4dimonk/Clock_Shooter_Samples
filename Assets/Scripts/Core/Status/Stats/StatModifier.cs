public enum StatModifierType
{
    Flat,
    Percent
}

public class StatModifier
{
    public StatType StatType;
    public float Value;
    public StatModifierType Type;
    public float DurationTurn;

    public bool IsPermanent => DurationTurn <= 0f;

    public StatModifier(StatType statType, float value, StatModifierType type, float durationTurn = 0f)
    {
        StatType = statType;
        Value = value;
        Type = type;
        DurationTurn = durationTurn;
    }
}
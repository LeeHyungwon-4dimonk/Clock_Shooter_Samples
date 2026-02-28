using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHP,
    AttackDamage,
    CritChance,
    CritDamageRate,
}

public struct AttackResult
{
    public int damage;
    public bool isCritical;
}

public class PlayerStats : Stats
{
    public int Level { get; private set; }
    public int Exp { get; private set; }
    public int[] RequiredExp { get; private set; }
    public bool IsAlive { get; private set; }

    public event Action OnLevelUp;

    private Dictionary<StatType, Stat> _stats;
    private StatModifier _stackAttackModifier;
    private TurnStack _boundTurnStack;
    private List<TurnStackAttackSegment> _stackSegment;

    #region Init

    public PlayerStats(PlayerStatData data)
    {
        Initialize(data);
    }

    public void Initialize(PlayerStatData data)
    {
        IsAlive = true;
        Level = data.StartLevel;
        Exp = 0;
        RequiredExp = data.RequiredExp;
        _stackSegment = data.Segment;

        InitStats(data);
        InitiailizeHP((int)GetStat(StatType.MaxHP));
    }

    private void InitStats(PlayerStatData data)
    {
        _stats = new Dictionary<StatType, Stat>
        {
            { StatType.MaxHP, new Stat(data.MaxHP) },
            { StatType.AttackDamage, new Stat(data.AttackDamage) },
            { StatType.CritChance, new Stat(data.CritChance) },
            { StatType.CritDamageRate, new Stat(data.CritDamage) },
        };

        _stackAttackModifier = new StatModifier(StatType.AttackDamage, 0f, StatModifierType.Flat, -1);

        _stats[StatType.AttackDamage].AddModifier(_stackAttackModifier);
    }

    public void BindTurnSystem(TurnSystem turn)
    {
        turn.OnTurnEnd += OnTurnEnd;
    }

    public void UnbindTurnSystem(TurnSystem turn)
    {
        turn.OnTurnEnd -= OnTurnEnd;
    }

    #endregion

    #region Stat

    public float GetStat(StatType type)
    {
        return _stats[type].Value;
    }

    public StatModifier AddStat(StatType type, float value)
    {
        return AddStat(type, value, StatModifierType.Flat, -1);
    }

    public StatModifier AddStat(StatType type, float value, StatModifierType modifierType, float duration)
    {
        if (!_stats.ContainsKey(type)) return null;

        var modifier = new StatModifier(type, value, modifierType, duration);
        _stats[type].AddModifier(modifier);
        if (type == StatType.MaxHP)
        {
            IncreaseMaxHP((int)value);
            RecoverHP((int)value);

            if (MaxHP >= 200) Manager.Steam.Achieve("ACH_HP_200");
        }

        NotifyStatusChanged();
        return modifier;
    }

    public void RemoveStat(StatModifier modifier)
    {
        if (!_stats.TryGetValue(modifier.StatType, out var stat))
            return;

        stat.RemoveModifier(modifier);
        NotifyStatusChanged();
    }

    private void OnTurnEnd()
    {
        foreach (var statPair in _stats)
        {
            var stat = statPair.Value;
            var modifiers = stat.GetModifiers();

            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                var mod = modifiers[i];
                if (mod.IsPermanent) continue;

                mod.DurationTurn--;

                if (mod.DurationTurn <= 0)
                {
                    stat.RemoveModifier(mod);
                    NotifyStatusChanged();
                }
            }
        }
    }

    #endregion

    #region TurnStack AttackBuff

    public void BindTurnStack(TurnStack turnStack)
    {
        if (_boundTurnStack == turnStack) return;

        UnbindTurnStack();

        _boundTurnStack = turnStack;
        _boundTurnStack.OnTurnValueChanged += OnTurnStackChanged;
    }

    public void UnbindTurnStack()
    {
        if (_boundTurnStack == null) return;

        _boundTurnStack.OnTurnValueChanged -= OnTurnStackChanged;
        _boundTurnStack = null;
    }

    private void OnTurnStackChanged(int prev, int current)
    {
        _stackAttackModifier.Value = CalculateTurnStackBuff(current);
        NotifyStatusChanged();
    }

    private float CalculateTurnStackBuff(int current)
    {
        float total = 0;
        int remaining = current;

        foreach (var segment in _stackSegment)
        {
            if (remaining <= 0) break;

            int applyCount = segment.StackCount < 0 ? remaining : Mathf.Min(remaining, segment.StackCount);

            total += applyCount * segment.AttackPerStack;
            remaining -= applyCount;
        }

        return total;
    }

    #endregion

    #region Attack Damage Calcualtion

    public AttackResult CalculateAttackResult()
    {
        bool isCritical = CritChance(GetStat(StatType.CritChance));

        int damage = isCritical
            ? (int)(GetStat(StatType.AttackDamage) * GetStat(StatType.CritDamageRate))
            : (int)GetStat(StatType.AttackDamage);

        return new AttackResult
        {
            damage = damage,
            isCritical = isCritical
        };
    }

    public bool CritChance(float prob)
    {
        float value = UnityEngine.Random.Range(0, 1f);

        if (value < prob) return true;
        else return false;
    }

    #endregion

    #region Recover

    public void Heal(int amount)
    {
        if (!IsAlive) return;
        RecoverHP(amount);
        NotifyStatusChanged();
    }

    public void HealPercent(float ratio)
    {
        int amount = (int)(GetStat(StatType.MaxHP) * ratio);
        Heal(amount);
    }

    #endregion

    #region Level & Exp

    public void AddExperience(int exp)
    {
        if (Level >= RequiredExp.Length + 1) return;

        Exp += exp;
        if (Exp >= RequiredExp[Level - 1])
        {
            LevelUp();
        }

        NotifyStatusChanged();
    }

    private void LevelUp()
    {
        if (Level >= RequiredExp.Length + 1)
        {
            Exp = 0;
            return;
        }
        Exp -= RequiredExp[Level - 1];
        Level++;
        OnLevelUp?.Invoke();
    }

    #endregion

    public void Die()
    {
        IsAlive = false;
    }
}
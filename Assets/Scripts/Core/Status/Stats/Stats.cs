using System;
using UnityEngine;

public abstract class Stats
{
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public event Action OnStatusChanged;

    protected void InitiailizeHP(int hp)
    {
        MaxHP = hp;
        CurrentHP = hp;
        OnStatusChanged?.Invoke();
    }

    protected void IncreaseMaxHP(int value)
    {
        MaxHP += value;
    }

    protected void RecoverHP(int value)
    {
        CurrentHP = Mathf.Min(CurrentHP + value, MaxHP);
    }

    public void TakeDamage(int damage)
    {
        CurrentHP = Mathf.Max(CurrentHP - damage, 0);
        OnStatusChanged?.Invoke();
    }

    protected void NotifyStatusChanged()
    {
        OnStatusChanged?.Invoke();
    }
}
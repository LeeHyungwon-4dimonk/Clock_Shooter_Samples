using System;
using UnityEngine;

public enum DamageSource
{
    DirectAttack,
    Poison
}

public class StatusManager : Singleton<StatusManager>
{
    public event Action OnMonsterDied;
    //public event Action OnPlayerDied;

    #region Status Changes(Battle)

    public void ApplyPlayerAttack(Monster monster, RaycastHit hit, float rate = 1)
    {
        monster.RequestDie();
        Manager.Game.AddKilledMonster();
        OnMonsterDied?.Invoke();
    }

    public void ApplyMonsterAttack(Monster monster)
    {
        monster.RequestDie();
        OnMonsterDied?.Invoke();
        Debug.Log("공격 받음");
    }

    #endregion
}
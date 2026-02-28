using System;
using System.Threading.Tasks;
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

    public PlayerStats PlyStats { get; private set; }
    public PlayerSkills PlySkills { get; private set; }
    public bool PlayerDamaged { get; private set; }

    private PlayerSkill[] _playerSkills;
    private PlayerStatData _data;
    private PlayerSkillSelector _selector;
    private SkillContext _cachedSkillContext;
    private ProjectileSystem _projectileSystem;

    public async Task Initialize()
    {
        await Manager.Data.WaitForReady();

        _data = Manager.Data.Get<PlayerStatData>();

        InitPlayerStatus();
        await InitSkillSelectorAsync();

        PlayerDamaged = false;

        IsInitialized = true;
    }

    #region Init

    public void InitPlayerStatus()
    {
        if (PlyStats == null) PlyStats = new PlayerStats(_data);
        else PlyStats.Initialize(_data);

        if (PlySkills == null) PlySkills = new PlayerSkills();
        PlySkills.RemoveAllPlayerSkills();

        if (_projectileSystem != null) _projectileSystem.ResetSystem();

        if (_playerSkills != null && _selector != null) _selector.InitSkills(_playerSkills);

        PlayerDamaged = false;    
    }

    private async Task InitSkillSelectorAsync()
    {
        _playerSkills = await Manager.Data.LoadAllSkillsAsync();
        _selector = new PlayerSkillSelector(_playerSkills);

        _selector.InitSkills(_playerSkills);
    }

    #endregion

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
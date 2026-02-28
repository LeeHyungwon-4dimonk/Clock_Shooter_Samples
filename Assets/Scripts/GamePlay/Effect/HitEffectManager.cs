using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum HitType
{
    Normal,
    Critical
}

public struct HitInfo
{
    public Vector3 position;
    public Vector3 normal;
    public HitType hitType;
    public AttackResult attackResult;
    public Monster target;
    public float rate;
}

public class HitEffectManager : Singleton<HitEffectManager>
{
    [Header("Hit Effect Data")]
    private HitEffectData _data;
    private Dictionary<HitType, HitEffectEntry> _effectMap;

    [Header("Damange UI")]
    [SerializeField] private GameObject _damageUICanvasPrefab;
    private DamageTextManager _damageTextManager;

    public async Task Initialize()
    {
        await Manager.Data.WaitForReady();

        InitEffectData();
        await InitDamageUI();
    }

    private void InitEffectData()
    {
        _data = Manager.Data.Get<HitEffectData>();
        _effectMap = new Dictionary<HitType, HitEffectEntry>();

        foreach (var entry in _data.entries)
        {
            _effectMap[entry.type] = entry;
            Manager.Pool.CreatePool(entry.poolKey, entry.prefab, 4, "EffectPool", transform);
        }
    }

    private async Task InitDamageUI()
    {
        _damageUICanvasPrefab = await AssetLoaderProvider.Loader.LoadAsync<GameObject>("DamageCanvas");

        var go = Instantiate(_damageUICanvasPrefab);
        _damageTextManager = go.GetComponent<DamageTextManager>();

        if (_damageTextManager == null)
            Debug.LogError("DamageTextManager가 DamageUICanvas에 없습니다.");
    }

    private void OnEnable()
    {
        GameEvents.OnMonsterHit += OnMonsterHit;
    }

    private void OnDisable()
    {
        GameEvents.OnMonsterHit -= OnMonsterHit;
    }

    private void OnMonsterHit(HitInfo hitInfo)
    {
        PlayHitEffect(hitInfo);
        PlayFireSound(hitInfo.attackResult);

        if (Manager.Game.isGamePaused)
            return;

        _damageTextManager?.Show(hitInfo);
    }

    private void PlayHitEffect(HitInfo hitInfo)
    {
        if (!_effectMap.TryGetValue(hitInfo.hitType, out var entry))
            return;

        var effect = Manager.Pool.Get(entry.poolKey);
        effect.transform.position = hitInfo.position;
        effect.transform.forward = hitInfo.normal;

        if (effect.TryGetComponent(out EffectReturnPool pool))
        {
            pool.SetPoolKey(entry.poolKey);
            pool.SetDuration(entry.duration);
        }
    }

    private void PlayFireSound(AttackResult result)
    {
        FireResultType type = result.isCritical ? FireResultType.Critical : FireResultType.Normal;
        Manager.Sound.PlayShootSound(type);
    }
}
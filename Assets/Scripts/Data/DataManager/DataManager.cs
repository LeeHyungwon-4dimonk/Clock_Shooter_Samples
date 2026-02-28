using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<Type, ScriptableObject> _cache = new Dictionary<Type, ScriptableObject>();

    private async Task<T> LoadAsync<T>(string key) where T : ScriptableObject
    {
        if (_cache.TryGetValue(typeof(T), out var cached))
            return cached as T;

        var loaded = await AssetLoaderProvider.Loader.LoadAsync<T>(key);

        if (loaded == null)
        {
            Debug.LogError($"[DataManager] Failed to Load {typeof(T)} : {key}");
            return null;
        }

        _cache[typeof(T)] = loaded;
        return loaded;
    }

    public async Task InitAllData()
    {
        IsInitialized = false;

        await LoadAsync<FireSoundData>("Data/FireSoundData");
        await LoadAsync<BGMSoundData>("Data/BGMSoundData");
        await LoadAsync<SFXSoundData>("Data/SFXSoundData");
        await LoadAsync<HitEffectData>("Data/HitEffectData");
        await LoadAsync<PlayerStatData>("Data/PlayerStatData");
        await LoadAsync<MonsterDatabase>("Data/MonsterDatabase");
        await LoadAsync<MonsterSummonPhase>("Data/MonsterSpawnRule");

        IsInitialized = true;
    }

    public async Task<PlayerSkill[]> LoadAllSkillsAsync()
    {
#if ADDRESSABLES_ENABLED
        var skills = await Addressables.LoadAssetsAsync<PlayerSkill>("Skills", null).Task;
#else
        var skills = await AssetLoaderProvider.Loader.LoadAllAsync<PlayerSkill>("Data/Skills/Skill");
#endif
        return skills.ToArray();
    }

    public async Task<PlayerSkill[]> LoadAllDebuffsAsync()
    {
#if ADDRESSABLES_ENABLED
        var debuffs = await Addressables.LoadAssetsAsync<PlayerSkill>("Debuffs",null).Task;
#else
        var debuffs = await AssetLoaderProvider.Loader.LoadAllAsync<PlayerSkill>("Data/Skills/Debuff");
#endif
        return debuffs.ToArray();
    }

    public T Get<T>() where T : ScriptableObject
    {
        if (_cache.TryGetValue(typeof(T), out var cached))
            return cached as T;

        Debug.LogError($"[DataManager] {typeof(T)} not loaded.");
        return null;
    }

    public void Release<T>() where T : ScriptableObject
    {
        if (!_cache.TryGetValue(typeof(T), out var data)) return;

        AssetLoaderProvider.Loader.Release(data);
        _cache.Remove(typeof(T));
    }
}
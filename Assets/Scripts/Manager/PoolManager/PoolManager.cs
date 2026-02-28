using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Pool> _poolsDict = new Dictionary<string, Pool>();

    public void CreatePool(string key, GameObject prefab, int size, string name = null, Transform parent = null)
    {
        if (_poolsDict.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key '{key}' already exists.");
            return;
        }

        Pool newPool = new Pool(key, prefab, size);

        newPool.Init(parent ? parent : transform, name);
        _poolsDict.Add(key, newPool);
    }

    public GameObject Get(string key)
    {
        if (!_poolsDict.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"Pool '{key}' not found");
            return null;
        }

        return pool.Get();
    }

    public GameObject Get(string key, Transform parent)
    {
        if (!_poolsDict.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"Pool '{key}' not found");
            return null;
        }

        return pool.Get(parent);
    }

    public void Return(string key, GameObject obj)
    {
        if (_poolsDict.TryGetValue(key, out var pool))
            pool.Return(obj);
    }

    public void ReleaseAll(string key)
    {
        if (!_poolsDict.TryGetValue(key, out var pool))
        {
            Debug.LogWarning($"Pool '{key}' not found");
            return;
        }

        pool.ReleaseAll();
    }

    public bool HasPool(string key)
    {
        return _poolsDict.ContainsKey(key);
    }
}
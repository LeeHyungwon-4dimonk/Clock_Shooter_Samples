using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Data/MonsterDatabase")]
public class MonsterDatabase : ScriptableObject
{
    public List<MonsterData> Monsters;

    private Dictionary<int, MonsterData> _cachedMap;

    public void BuildCache()
    {
        _cachedMap = new Dictionary<int, MonsterData>();
        foreach (var monster in Monsters)
            _cachedMap[monster.ID] = monster;
    }

    public MonsterData Get(int id)
    {
        if (_cachedMap == null)
            BuildCache();

        if (_cachedMap.TryGetValue(id, out var data))
            return data;

        Debug.LogError($"MonsterData not found : {id}");
        return null;
    }
}
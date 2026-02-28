using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMData", menuName = "Sound/BGMData")]
public class BGMSoundData : ScriptableObject
{
    public BGMEntry[] entries;

    private Dictionary<ConditionType, BGMEntry> _map;

    public void Init()
    {
        _map = new Dictionary<ConditionType, BGMEntry>();
        foreach (var e in entries)
            _map[e.type] = e;
    }

    public BGMEntry Get(ConditionType type)
    {
        if (_map == null) Init();
        return _map.TryGetValue(type, out var e) ? e : null;
    }
}

[System.Serializable]
public class BGMEntry
{
    public ConditionType type;
    public AudioClip clip;
}

public enum ConditionType
{
    Intro,
    Ingame,
    Boss
}
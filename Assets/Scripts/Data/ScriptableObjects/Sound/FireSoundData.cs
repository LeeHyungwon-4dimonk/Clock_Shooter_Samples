using UnityEngine;
using System.Collections.Generic;

public enum FireResultType
{
    Miss,
    Normal,
    Critical
}

[System.Serializable]
public class FireSoundEntry
{
    public FireResultType resultType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
}

[CreateAssetMenu(fileName = "FireSoundData", menuName = "Sound/FireSoundData")]
public class FireSoundData : ScriptableObject
{
    public FireSoundEntry[] entries;

    private Dictionary<FireResultType, FireSoundEntry> _map;

    public void Init()
    {
        _map = new Dictionary<FireResultType, FireSoundEntry>();
        foreach (var e in entries)
            _map[e.resultType] = e;
    }

    public FireSoundEntry Get(FireResultType type)
    {
        if (_map == null) Init();
        return _map.TryGetValue(type, out var e) ? e : null;
    }
}
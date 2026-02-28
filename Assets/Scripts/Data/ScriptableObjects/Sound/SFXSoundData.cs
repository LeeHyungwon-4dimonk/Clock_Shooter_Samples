using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    DragonRoar,
    DragonFly,
    DragonDamaged,
    Charge,
    BossHit
}

[System.Serializable]
public class SFXEntry
{
    public SFXType Type;
    public AudioClip Clip;
}

[CreateAssetMenu(menuName = "Sound/SFXData")]
public class SFXSoundData : ScriptableObject
{
    public SFXEntry[] entries;

    private Dictionary<SFXType, SFXEntry> _map;

    public void Init()
    {
        _map = new Dictionary<SFXType, SFXEntry>();
        foreach (var e in entries)
            _map[e.Type] = e;
    }

    public SFXEntry Get(SFXType type)
    {
        if (_map == null) Init();
        return _map.TryGetValue(type, out var e) ? e : null;
    }
}
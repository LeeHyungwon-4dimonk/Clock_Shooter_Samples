using UnityEngine;

[CreateAssetMenu(fileName = "HitEffectData", menuName = "Effects/HitEffectData")]
public class HitEffectData : ScriptableObject
{
    public HitEffectEntry[] entries;
}

[System.Serializable]
public class HitEffectEntry
{
    public HitType type;
    public string poolKey;
    public GameObject prefab;
    public float duration = 0.5f;
}
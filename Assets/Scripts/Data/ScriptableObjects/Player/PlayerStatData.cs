using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Stats/Player")]
public class PlayerStatData : ScriptableObject
{
    public int MaxHP;
    public int AttackDamage;
    public float CritChance;
    public float CritDamage;
    public int StartLevel;
    public int[] RequiredExp;

    public List<TurnStackAttackSegment> Segment;

    public int GetSegmentStackCountSum()
    {
        int sum = 0;
        foreach (var seg in Segment)
        {
            sum += seg.StackCount;
        }
        return sum + 1;
    }
}

[System.Serializable]
public class TurnStackAttackSegment
{
    public int StackCount;
    public float AttackPerStack;
}
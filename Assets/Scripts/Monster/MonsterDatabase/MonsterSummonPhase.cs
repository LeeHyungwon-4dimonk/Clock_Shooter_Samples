using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Data/MonsterSpawnRule")]
public class MonsterSummonPhase : ScriptableObject
{
    public List<PhaseWeight> SummonWeights;
}

[System.Serializable]
public class PhaseWeight
{
    public int PhaseKillNum;
    public List<MonsterWeight> MonsterWeights;
}

[System.Serializable]
public class MonsterWeight
{
    public MonsterData MonsterID;
    public int Weight;
}
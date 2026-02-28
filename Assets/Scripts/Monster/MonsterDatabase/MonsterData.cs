using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    public int ID;
    public string Name;
    [TextArea] public string Description;

    public GameObject Prefab;
    //public MonsterStatData StatData;
}
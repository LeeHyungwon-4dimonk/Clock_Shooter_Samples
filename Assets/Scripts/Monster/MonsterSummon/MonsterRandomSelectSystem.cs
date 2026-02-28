public class MonsterRandomSelectSystem
{
    private MonsterSummonPhase _data;
    private WeightedRandom<MonsterData> _random = new();
    private int _currentPhase = 0;

    public MonsterRandomSelectSystem(MonsterSummonPhase data)
    {
        _data = data;
        InitWeightRandom();
    }

    private void InitWeightRandom()
    {
        _random.ClearList();

        foreach (var monster in _data.SummonWeights[_currentPhase].MonsterWeights)
        {
            _random.Add(monster.MonsterID, monster.Weight);
        }
    }

    public MonsterData GetRandomData()
    {
        int currentPhase = GetPhase();

        if (_currentPhase != currentPhase)
        {
            _currentPhase = currentPhase;
            InitWeightRandom();
            GameEvents.RaiseMonsterPhaseChanged(_currentPhase + 1);
        }

        MonsterData monster = _random.GetRandom();

        return monster;
    }

    private int GetPhase()
    {
        int killedNum = Manager.Game.KilledMonsterNum;
        for (int i = 0; i < _data.SummonWeights.Count; i++)
        {
            if (killedNum <= _data.SummonWeights[i].PhaseKillNum)
                return i;
            killedNum -= _data.SummonWeights[i].PhaseKillNum;
        }
        return _data.SummonWeights.Count - 1;
    }

    public void InitPhase()
    {
        _currentPhase = 0;
        InitWeightRandom();
    }
}
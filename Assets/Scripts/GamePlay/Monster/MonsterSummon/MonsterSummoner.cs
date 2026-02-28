using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterSummoner : MonoBehaviour
{
    [Header("Offset")]
    [SerializeField] private int _maxSummonNum = 8;
    [SerializeField] private int _directionCount = 8;
    [SerializeField] private int _summonTurnOffset = 2;

    [Header("Data")]
    [SerializeField] MonsterDatabase _data;
    [SerializeField] MonsterSummonPhase _phaseData;
    
    private int _summonTurn = 2;
    private int _summonNum;
    private MonsterRandomSelectSystem _random;

    private async void Start()
    {
        while (!ManagerBootstrapper.IsBootstrapped)
            await Task.Yield();

        InitAsync();
        _random = new MonsterRandomSelectSystem(_phaseData);

        await Manager.Game.WaitForReady();

        Bind();
    }

    // TODO : Lazy Pooling
    private void InitAsync()
    {
        for (int i = 0; i < _data.Monsters.Count; i++)
        {
            var data = _data.Monsters[i];

            if (data != null)
            {
                Manager.Pool.CreatePool(data.Name, data.Prefab,
                    _maxSummonNum, data.ID.ToString(), transform);
            }
        }
    }

    private void Bind()
    {
        Manager.Game.turnStack.OnTurnValueChanged += OnTurnValueChanged;
        Manager.Game.turnStack.OnTurnTick += OnTurnTick;

        Manager.Status.OnMonsterDied += OnMonsterDied;
        Manager.Game.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        Manager.Game.turnStack.OnTurnValueChanged -= OnTurnValueChanged;
        Manager.Game.turnStack.OnTurnTick -= OnTurnTick;

        Manager.Status.OnMonsterDied -= OnMonsterDied;
        Manager.Game.OnGameOver -= OnGameOver;
    }

    private void OnTurnValueChanged(int prev, int cur)
    {
        if (prev == -1) return;

        int delta = cur - prev;

        Manager.Game.monsterPositionManager.ResolveTurnMove(delta);
    }

    private void OnTurnTick(int currentTurn)
    {
        _summonTurn++;

        if (_summonNum >= _maxSummonNum) return;
        if (_summonTurn < _summonTurnOffset) return;

        GameObject monster = SummonRandomMonster(out MonsterData stats);

        if (monster.TryGetComponent(out Monster comp))
            comp.InitData(stats);

        int dir = Random.Range(0, _directionCount);
        monster.GetComponent<MonsterController>().Initialize(dir);

        _summonNum++;
        _summonTurn = 0;
    }

    private GameObject SummonRandomMonster(out MonsterData data)
    {
        data = _random.GetRandomData();

        return Manager.Pool.Get(data.Name);
    }

    private void OnMonsterDied()
    {
        _summonNum = Mathf.Max(0, _summonNum - 1);
    }

    private void OnGameOver()
    {
        _summonTurn = 2;
        _summonNum = 0;
        _random.InitPhase();
    }
}
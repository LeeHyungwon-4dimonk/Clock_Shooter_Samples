using UnityEngine;

public enum MonsterLifeState
{
    Alive,
    Dead
}

public class Monster : MonoBehaviour, IPoolable
{
    public MonsterLifeState LifeState { get; private set; } = MonsterLifeState.Alive;

    public bool IsInitialized { get; private set; } = false;

    private MonsterData _data;

    private MonsterController _controller;
    private bool _subscribed;
    private bool _deathCommited = false;


    private void Awake()
    {
        _controller = GetComponent<MonsterController>();
    }

    private void OnEnable()
    {
        Manager.Game.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        Manager.Game.OnGameOver -= OnGameOver;
    }

    private void LateUpdate()
    {
        if (LifeState == MonsterLifeState.Dead && !_deathCommited)
        {
            _deathCommited = true;
            Die();
        }
    }

    #region Init

    public void InitData(MonsterData data)
    {
        _data = data;
        _deathCommited = false;
        IsInitialized = true;
    }

    #endregion

    #region PoolEvent

    public void OnSpawned()
    {
        if (_subscribed) return;

        LifeState = MonsterLifeState.Alive;
        _controller.OnHit += HandleHit;
        _subscribed = true;
    }

    public void OnReturned()
    {
        if (!_subscribed) return;

        Manager.Game.monsterPositionManager.Unregister(_controller);

        _data = null;
        _controller.OnHit -= HandleHit;
        _subscribed = false;
        IsInitialized = false;
    }

    #endregion

    #region HitEvent

    private void HandleHit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Manager.Status.ApplyMonsterAttack(this);
    }

    #endregion

    #region Die / GameOver

    public void RequestDie()
    {
        if (LifeState != MonsterLifeState.Alive) return;

        LifeState = MonsterLifeState.Dead;
    }

    private void Die()
    {
        Manager.Game.monsterPositionManager.Unregister(_controller);
        Manager.Pool.Return(_data.Name, gameObject);
    }

    private void OnGameOver()
    {
        if (_deathCommited) return;

        Manager.Game.monsterPositionManager.Unregister(_controller);
        Manager.Pool.Return(_data.Name, gameObject);
    }

    #endregion
}
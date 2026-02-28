using System;
using System.Threading.Tasks;
using UnityEngine;

public class BossMonsterState : MonoBehaviour
{
    [SerializeField] GameObject _boss;

    private BossMonsterStateMachine _stateMachine;
    private BossMonsterAnimator _animator;

    private void Awake()
    {
        _stateMachine = GetComponent<BossMonsterStateMachine>();
        _animator = GetComponent<BossMonsterAnimator>();
    }

    private async void Start()
    {
        _boss.SetActive(false);

        while (!ManagerBootstrapper.IsBootstrapped)
            await Task.Yield();

        await Manager.Game.WaitForReady();

        Manager.Game.OnGameOver += Init;
    }
    private void OnDestroy()
    {
        Manager.Game.OnGameOver -= Init;
    }

    private void Init()
    {
        if (Manager.Game.isBossPhase) _boss.SetActive(true);
        else _boss.SetActive(false);
    }

    public void PlayAppear()
    {
        _boss.SetActive(true);

        _animator.PlayAppearCinematic(
            onStart: () =>
            {
                _stateMachine.FSM.ChangeState<BossMonsterAppearState>();
            },
            onEnd: () =>
            {
                _stateMachine.FSM.ChangeState<BossMonsterIdleState>();
            });
    }

    public void PlayDamage(Action onCinematicEnd)
    {
        _animator.PlayDamageCinematic(
            onStart: () =>
            {
                _stateMachine.FSM.ChangeState<BossMonsterDamagedState>();
            },
            onEnd: () =>
            {
                _stateMachine.FSM.ChangeState<BossMonsterIdleState>();

                onCinematicEnd?.Invoke();
            });
    }

    public void PlayRunaway(Action onCinematicEnd)
    {
        _animator.PlayRunawayCinematic(
            onStart: () =>
            {
                _stateMachine.FSM.ChangeState<BossMonsterRunawayState>();
            },
            onEnd: () =>
            {
                _boss.SetActive(false);

                onCinematicEnd?.Invoke();
            });
    }
}
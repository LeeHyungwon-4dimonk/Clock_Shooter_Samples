public class PlayerIdleState : State
{
    private PlayerStateMachine _player;

    public PlayerIdleState(PlayerStateMachine player, StateMachine fsm) : base(fsm)
    {
        _player = player;
    }


    public override void Enter()
    {

    }
}

public class PlayerShootState : State
{
    private PlayerStateMachine _player;

    public PlayerShootState(PlayerStateMachine player, StateMachine fsm) : base(fsm)
    {
        _player = player;
    }

    public override void Enter()
    {
        _player.Animator.SetTrigger("Shoot");
    }
}

public class PlayerDieState : State
{
    private PlayerStateMachine _player;

    public PlayerDieState(PlayerStateMachine player, StateMachine fsm) : base(fsm)
    {
        _player = player;
    }

    public override void Enter()
    {
        _player.Animator.SetTrigger("Die");
    }
}
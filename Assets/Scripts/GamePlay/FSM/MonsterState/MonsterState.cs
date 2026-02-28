public class MonsterIdleState : State
{
    private MonsterStateMachine _monster;

    public MonsterIdleState(MonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetBool("IsWalking", false);
    }
}

public class MonsterWalkState : State
{
    private MonsterStateMachine _monster;

    public MonsterWalkState(MonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetBool("IsWalking", true);
    }

    public override void Exit()
    {
        _monster.Animator.SetBool("IsWalking", false);
    }
}
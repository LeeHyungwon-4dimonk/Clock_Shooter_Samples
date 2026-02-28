public class BossMonsterAppearState : State
{
    private BossMonsterStateMachine _monster;

    public BossMonsterAppearState(BossMonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetBool("IsAppeared", true);
    }

    public override void Exit()
    {
        _monster.Animator.SetBool("IsAppeared", false);
    }
}

public class BossMonsterIdleState : State
{
    private BossMonsterStateMachine _monster;

    public BossMonsterIdleState(BossMonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {

    }
}

public class BossMonsterScreamState : State
{
    private BossMonsterStateMachine _monster;

    public BossMonsterScreamState(BossMonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetTrigger("OnScream");
    }
}

public class BossMonsterDamagedState : State
{
    private BossMonsterStateMachine _monster;

    public BossMonsterDamagedState(BossMonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetTrigger("OnDamaged");
    }
}

public class BossMonsterRunawayState : State
{
    private BossMonsterStateMachine _monster;

    public BossMonsterRunawayState(BossMonsterStateMachine monster, StateMachine fsm) : base(fsm)
    {
        _monster = monster;
    }

    public override void Enter()
    {
        _monster.Animator.SetTrigger("OnDied");
    }
}
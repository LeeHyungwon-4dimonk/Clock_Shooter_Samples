public abstract class State
{
    protected StateMachine _fsm;

    protected State(StateMachine fsm)
    {
        _fsm = fsm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
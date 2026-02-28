using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public StateMachine FSM { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        FSM = new StateMachine();

        FSM.AddState(new PlayerIdleState(this, FSM));
        FSM.AddState(new PlayerShootState(this, FSM));
        FSM.AddState(new PlayerDieState(this, FSM));

        FSM.ChangeState<PlayerIdleState>();
    }

    private void Update()
    {
        FSM.Update();
    }
}

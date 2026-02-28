using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    public StateMachine FSM { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        FSM = new StateMachine();

        FSM.AddState(new MonsterIdleState(this, FSM));
        FSM.AddState(new MonsterWalkState(this, FSM));

        FSM.ChangeState<MonsterIdleState>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        FSM.Update();
    }
}

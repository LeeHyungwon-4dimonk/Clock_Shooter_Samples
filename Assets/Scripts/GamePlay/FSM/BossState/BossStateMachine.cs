using UnityEngine;

public class BossMonsterStateMachine : MonoBehaviour
{
    public StateMachine FSM { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        FSM = new StateMachine();

        FSM.AddState(new BossMonsterAppearState(this, FSM));
        FSM.AddState(new BossMonsterIdleState(this, FSM));
        FSM.AddState(new BossMonsterScreamState(this, FSM));
        FSM.AddState(new BossMonsterDamagedState(this, FSM));
        FSM.AddState(new BossMonsterRunawayState(this, FSM));
    }

    private void Start()
    {

    }

    private void Update()
    {
        FSM.Update();
    }
}

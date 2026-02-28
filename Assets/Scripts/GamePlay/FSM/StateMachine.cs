using System;
using System.Collections.Generic;

public class StateMachine
{
    private State _currentState;
    private Dictionary<Type, State> _states = new();

    public void AddState(State state)
    {
        _states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : State
    {
        Type type = typeof(T);

        if (_currentState != null && _currentState.GetType() == type)
            return;

        _currentState?.Exit();
        _currentState = _states[typeof(T)];
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
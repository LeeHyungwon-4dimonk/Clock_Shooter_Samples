using System;
using UnityEngine;

public class TurnSystem
{
    public int CurrentTurn => _stack.Current;

    public event Action OnTurnEnd;
    public event Action OnTurnTick;
    public event Action<int, int> OnTurnChanged;

    private TurnStack _stack;

    public TurnSystem(TurnStack stack)
    {
        _stack = stack;

        _stack.OnTurnEnd += () => OnTurnEnd?.Invoke();
        _stack.OnTurnTick += _ => OnTurnTick?.Invoke();
        _stack.OnTurnValueChanged += (p, c) => OnTurnChanged?.Invoke(p, c);
    }
}
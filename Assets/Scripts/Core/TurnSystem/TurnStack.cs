using System;
using System.Collections.Generic;

public class TurnStack
{
    public int Current { get; private set; }
    public int TotalTurn { get; private set; }

    public Func<int, int> ModifyTurnDelta;

    public event Action<int, int> OnTurnValueChanged;
    public event Action<int> OnTurnTick;
    public event Action OnTurnEnd;

    private readonly Queue<PlayerAction> _actions = new();

    public TurnStack(int initialValue = 0, int initialTotalValue = 0)
    {
        Current = initialValue;
        TotalTurn = initialTotalValue;
    }

    public void Initialize()
    {
        Current = 0;
        TotalTurn = 0;
        OnTurnValueChanged?.Invoke(Current - 1, Current);
    }

    public void Enqueue(PlayerAction action)
    {
        if (Manager.Game.isGamePaused)
        {
            _actions.Clear();
            return;
        }

        _actions.Enqueue(action);
    }

    public void Resolve()
    {
        while (_actions.Count > 0)
        {
            PlayerAction action = _actions.Dequeue();

            action.Execute?.Invoke();

            if (action.TurnDelta != 0)
            {
                int prev = Current;
                int delta = action.TurnDelta;

                if (ModifyTurnDelta != null)
                {
                    delta = ModifyTurnDelta.Invoke(delta);
                }

                Current += delta;
                OnTurnValueChanged?.Invoke(prev, Current);
            }

            if (action.GenerateTick && action.TurnDelta == 0)
            {
                OnTurnValueChanged?.Invoke(Current, Current + 1);
                OnTurnTick?.Invoke(Current);
            }
            else if (action.GenerateTick)
            {
                OnTurnTick?.Invoke(Current);
            }

            OnTurnEnd?.Invoke();
            TotalTurn++;
        }
    }
}

public class PlayerAction
{
    public Action Execute;
    public int TurnDelta;
    public bool GenerateTick;

    public PlayerAction(Action execute, int turnDelta, bool generateTick)
    {
        Execute = execute;
        TurnDelta = turnDelta;
        GenerateTick = generateTick;
    }
}
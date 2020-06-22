using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }
    public State previousState { get; private set; }

    public StateMachine()
    {
        currentState = null;
    }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        startingState.EnterState();
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
           { currentState.ExitState(); }
        previousState = currentState;
        currentState = newState;
        currentState.EnterState();
    }

    public void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected AI owner;
    protected StateMachine stateMachine;

    public State(AI owner, StateMachine stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

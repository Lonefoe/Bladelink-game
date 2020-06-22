using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    private Enemy enemy;

    public DeadState(AI owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
    }

    public override void EnterState()
    {
        
    }
    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (!enemy.IsDead()) stateMachine.ChangeState(owner.idleState);
    }
}

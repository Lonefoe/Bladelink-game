using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    private Enemy enemy;

    public WanderState(AI owner, StateMachine stateMachine) : base(owner, stateMachine)
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
        if(enemy.IsDead()) stateMachine.ChangeState(owner.deadState);

        enemy.Movement.moveInput = 1f;

        if (owner.sight.CanSeePlayer() && !Player.Instance.IsDead())
        {
            stateMachine.ChangeState(owner.chaseState);
        }
    }
}

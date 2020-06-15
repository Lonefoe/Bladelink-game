using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private Enemy enemy;
    private float chaseRange;

    public event Action attackEvent;

    public ChaseState(AI owner, StateMachine stateMachine, float chaseRange) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
        this.chaseRange = chaseRange;
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (Player.Instance.IsDead()) stateMachine.ChangeState(owner.patrolState);

        if (!enemy.Attack.IsAttacking()) { enemy.Movement.moveInput = 1; enemy.Movement.UpdateDirection(Player.Instance.GetPosition()); }

        if (enemy.IsPlayerInRange(enemy.Attack.attackRange))
        {
            if (!enemy.Attack.IsAttacking())
            {
                enemy.Animator.SetTrigger("Attack");
                if (attackEvent != null) attackEvent();
            }
            enemy.Movement.moveInput = 0;
        }

        if (!owner.sight.CanSeePlayer() && !IsInChaseRange()) stateMachine.ChangeState(owner.patrolState);
    }

    private bool IsInChaseRange()
    {
        if (Vector2.Distance(enemy.GetPosition(), Player.Instance.GetPosition()) <= chaseRange)
        {
            return true;
        }
        else return false;
    }

}

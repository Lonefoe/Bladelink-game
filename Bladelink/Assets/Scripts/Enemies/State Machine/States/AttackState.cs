using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackState : State
{
    private Enemy enemy;
    public event Action attackEvent;
    public AttackState(AI owner, StateMachine stateMachine) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
    }

    public override void EnterState()
    {
        enemy.Animator.SetTrigger("Attack");
        if (attackEvent != null) attackEvent();
        enemy.Movement.moveInput = 0;
    }

    public override void ExitState()
    {
        owner.timeSinceLastAttack = 0;
    }

    public override void UpdateState()
    {
      //  enemy.Movement.UpdateDirection(Player.Instance.GetPosition());
    }
}

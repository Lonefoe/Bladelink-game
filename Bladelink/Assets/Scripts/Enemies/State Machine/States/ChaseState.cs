using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private Enemy enemy;
    private bool allyDetected, playerUnreachable;
    private ChaseState_Data data;

    public ChaseState(AI owner, StateMachine stateMachine, ChaseState_Data chaseState_Data) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
        data = chaseState_Data; 
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        enemy.Controller.Strafe(false);
    }

    public override void UpdateState()
    {
        // TRANSITIONS
        if (enemy.IsDead()) { stateMachine.ChangeState(owner.deadState); return; }
        if (!owner.sight.CanSeePlayer() && !IsInRange(data.chaseRange) || Player.Instance.IsDead()) { stateMachine.ChangeState(owner.patrolState); return; }

        // LOGIC
        enemy.Movement.moveInput = 0.9f;

        if (enemy.IsPlayerInRange(enemy.Attack.attackDetectionRange))
        {
            enemy.Movement.moveInput = 0f;
            if(owner.timeSinceLastAttack > UnityEngine.Random.Range(data.minAttackDelay, data.maxAttackDelay))
            {
                stateMachine.ChangeState(owner.attackState);
            }
        }
        playerUnreachable = Physics2D.Raycast(enemy.transform.position, Vector2.up, 3f,data.whatIsPlayer);

        if(!playerUnreachable)
        {
        enemy.Controller.Strafe(true);
        enemy.Movement.Face(Player.Instance.gameObject);
        }
        else enemy.Movement.moveInput = 0;
        
        allyDetected = Physics2D.Linecast(enemy.transform.position + (new Vector3(0.5f, 0f, 0f) * enemy.Movement.GetDirection()), Player.Instance.GetPosition(), data.whatIsEnemy);
        if(allyDetected) enemy.Movement.moveInput = 0f;
    }

    private bool IsInRange(float range)
    {
        if (Vector2.Distance(enemy.GetPosition(), Player.Instance.GetPosition()) <= range)
        {
            return true;
        }
        else return false;
    }

}

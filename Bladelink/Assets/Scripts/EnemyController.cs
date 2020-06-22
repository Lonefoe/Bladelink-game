using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
	[Header("Enemy Controller")]
	public Transform wallCheck;
	public Transform ledgeCheck;
	private bool wallDetected, ledgeDetected;
	private Enemy enemy;

	private void Start() {
		enemy = GetComponent<Enemy>();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
		
		wallDetected = Physics2D.Raycast(wallCheck.position, transform.right * enemy.Movement.GetDirection(), 0.3f, m_WhatIsGround);
		ledgeDetected = !Physics2D.Raycast(ledgeCheck.position, transform.up * -1, 0.3f, m_WhatIsGround);

		if(wallDetected || ledgeDetected) { enemy.AI.stateMachine.ChangeState(enemy.AI.wanderState); enemy.Movement.Flip(); 
		Invoke("ChangeToPatrolState", 1f); }
    }
	
	private void ChangeToPatrolState()
	{
		enemy.AI.stateMachine.ChangeState(enemy.AI.patrolState);
	}
    
	public void Strafe(bool strafe)
	{
		if(strafe) m_Strafing = true;
		else m_Strafing = false;
	}

}

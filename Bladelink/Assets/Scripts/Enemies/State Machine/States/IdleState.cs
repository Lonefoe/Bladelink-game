using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private Enemy enemy;
    [SerializeField] private List<float> waitTimes;
    private int waitTimeIndex = -1;
    private float waitTime;

    public IdleState(AI owner, StateMachine stateMachine, List<float> waitTimes) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
        this.waitTimes = waitTimes;
    }

    public override void EnterState()
    {
        waitTimeIndex = GetNextTimeIndex(); // Update the index of the list with wait times
        waitTime = waitTimes[waitTimeIndex]; // Update the next time to wait with a new amount
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (waitTime <= 0) stateMachine.ChangeState(owner.patrolState);
        else { enemy.Movement.moveInput = 0; waitTime -= Time.deltaTime; }

        if (owner.sight.CanSeePlayer()) stateMachine.ChangeState(owner.chaseState);
    }


    private int GetNextTimeIndex()
    {
        if (waitTimeIndex == waitTimes.Count - 1) return 0;
        else return waitTimeIndex + 1;
    }
}

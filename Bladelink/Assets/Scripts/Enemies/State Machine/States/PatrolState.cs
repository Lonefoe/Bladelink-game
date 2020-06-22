using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    private Enemy enemy;
    private MovementType movementType;
    [SerializeField] private EnemyPath path;
    private Vector2 stationaryPoint;
    private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int startPathIndex = 1;
    private int currentPathIndex;   // Current index we're moving towards

    public PatrolState(AI owner, StateMachine stateMachine, MovementType movementType, EnemyPath path, int startPathIndex) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
        this.path = path;
        this.startPathIndex = startPathIndex;
        this.movementType = movementType;

        if (path != null) pathPoints = path.GetPoints();
        currentPathIndex = startPathIndex - 1;
        if(movementType == MovementType.Stationary) stationaryPoint = enemy.transform.position;
    }

    public override void EnterState()
    {
        if (movementType == MovementType.Stationary) { enemy.Movement.moveInput = 1; enemy.Movement.Flip(); return; }
        if (movementType == MovementType.Walker) currentPathIndex = GetNextPointIndex();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if(enemy.IsDead()) stateMachine.ChangeState(owner.deadState);

        if (movementType == MovementType.Stationary) { NoPathPatrol(); return; }

        var enemyPos = new Vector2(enemy.transform.position.x, 0);
        var pointPos = new Vector2(pathPoints[currentPathIndex].position.x, 0);

        if (Vector2.Distance(enemyPos, pointPos) < 0.2f)
        {
            stateMachine.ChangeState(owner.idleState); 
        }

        enemy.Movement.UpdateDirection(pathPoints[currentPathIndex].position);
        enemy.Movement.moveInput = 1;

        if (owner.sight.CanSeePlayer() && !Player.Instance.IsDead())
        {
            stateMachine.ChangeState(owner.chaseState);
        }

    }

    private void NoPathPatrol()
    {
        if(enemy.IsDead()) stateMachine.ChangeState(owner.deadState);

        if (Vector2.Distance(enemy.GetPosition(), stationaryPoint) < 0.2f)
        {
            stateMachine.ChangeState(owner.idleState); 
        }

        if (owner.sight.CanSeePlayer() && !Player.Instance.IsDead())
        {
            stateMachine.ChangeState(owner.chaseState);
        }

        enemy.Movement.UpdateDirection(stationaryPoint);
    }

    private int GetNextPointIndex()
    {
        if (currentPathIndex == 0)
        {
            return pathPoints.Count - 1;
        }
        else if (currentPathIndex == pathPoints.Count - 1)
        {
            return 0;
        }
        else
        {
            return 0;
        }

    }
}

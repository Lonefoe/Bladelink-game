using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    private Enemy enemy;
    [SerializeField] private EnemyPath path;
    private bool noPathPatrol = false;
    private Vector2 startPos;
    private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int startPathIndex = 1;
    private int currentPathIndex;   // Current index we're moving towards

    public PatrolState(AI owner, StateMachine stateMachine, EnemyPath path, int startPathIndex) : base(owner, stateMachine)
    {
        enemy = owner.GetComponent<Enemy>();
        this.path = path;
        this.startPathIndex = startPathIndex;

        if (path != null) pathPoints = path.GetPoints();
        currentPathIndex = startPathIndex - 1;
        startPos = enemy.GetPosition();
    }

    public override void EnterState()
    {
        if (path == null) { enemy.Movement.Flip(); enemy.Movement.moveInput = 1; NoPathPatrol(); return; }
        currentPathIndex = GetNextPointIndex();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (noPathPatrol) { return; }
        var enemyPos = new Vector2(enemy.transform.position.x, 0);
        var pointPos = new Vector2(pathPoints[currentPathIndex].position.x, 0);

        if (Vector2.Distance(enemyPos, pointPos) < 0.2f)
        {
            stateMachine.ChangeState(owner.idleState); 
        }

        enemy.Movement.UpdateDirection(pathPoints[currentPathIndex].position);
        enemy.Movement.moveInput = 1;

        if (owner.sight.CanSeePlayer())
        {
            stateMachine.ChangeState(owner.chaseState);
        }

    }

    private void NoPathPatrol()
    {
        noPathPatrol = true;
        enemy.transform.position = startPos;
        stateMachine.ChangeState(owner.idleState);
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

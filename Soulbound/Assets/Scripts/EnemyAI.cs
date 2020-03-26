using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private EnemyMovement movement;

    private enum State
    {
        Patrolling,
        Chasing,
    }

    [SerializeField] private Path path;
    private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int startPathIndex = 1;
    private int currentPathIndex;   // Current index we're moving towards
    private int direction = 1;

    private State state;
    [SerializeField] private float targetRange = 1f;
   

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        state = State.Patrolling;
        pathPoints = path.GetPoints();
        currentPathIndex = startPathIndex;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Player.Instance.GetPosition()) < targetRange)
        {
            state = State.Chasing;
        }
        else state = State.Patrolling;

        switch (state)
        {
            case State.Patrolling:
                Patrol();
                break;

            case State.Chasing:
                Chase();
                break;
        }
    }


    private void Patrol()
    {
        var enemyPos = new Vector2(transform.position.x, 0);
        var pointPos = new Vector2(pathPoints[currentPathIndex].position.x, 0);

        movement.ChangeDirection(pathPoints[currentPathIndex].position);

        if (Vector2.Distance(enemyPos, pointPos) < 0.2f)
        {
            currentPathIndex = GetNextPointIndex();
        }

    }

    private void Chase()
    {
        movement.ChangeDirection(Player.Instance.GetPosition());
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }

}

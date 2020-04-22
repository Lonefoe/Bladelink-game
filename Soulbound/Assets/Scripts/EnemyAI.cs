using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField] private Path path;
    private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int startPathIndex = 1;
    private int currentPathIndex;   // Current index we're moving towards

    private State state;
    [SerializeField] private float targetRange = 4f;
    private float attackRange = 0.8f;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemy.onDeathEvent += OnPlayerDeath;

        state = State.Patrolling;
        pathPoints = path.GetPoints();
        currentPathIndex = startPathIndex;
    }

    private void Update()
    {
        if (state == State.Dead) return;

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

        enemy.Movement.ChangeDirection(pathPoints[currentPathIndex].position);

        if (Vector2.Distance(enemyPos, pointPos) < 0.2f)
        {
            currentPathIndex = GetNextPointIndex();
        }

    }

    private void Chase()
    {
        enemy.Movement.ChangeDirection(Player.Instance.GetPosition());

        if (Vector2.Distance(enemy.Attack.attackPoint.position, Player.Instance.GetPosition()) < attackRange)
        {
            if (!enemy.Attack.IsAttacking())
            {
                enemy.Animator.SetTrigger("Attack");
                enemy.Movement.MoveSpeedMultiplier = 0f;
            }
        }

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

    private void OnPlayerDeath()
    {
        state = State.Dead;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, targetRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.up);
    }

}

public enum State
{
    Patrolling,
    Chasing,
    Dead,
}

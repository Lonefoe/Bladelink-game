using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Senses;

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    [Header("Pathing")]
    [SerializeField] private Path path;
    private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private int startPathIndex = 1;
    private int currentPathIndex;   // Current index we're moving towards

    [Header("Waiting")]
    [SerializeField] private float startWaitTime = 0;
    private float waitTime;    

    private State state = State.Patrolling;

    [Header("Range")]
    [SerializeField] private EnemySight sight;
    [SerializeField] private float chaseRange = 8f;
    private float attackRange = 0.8f;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemy.onDeathEvent += OnDeath;
        enemy.Senses.onTakeDamageEvent += OnTakeDamage;

        state = State.Patrolling;
        pathPoints = path.GetPoints();
        currentPathIndex = startPathIndex;
        waitTime = startWaitTime;
    }

    private void LateUpdate()
    {
        if (state == State.Dead) return;

         if (sight.CanSeePlayer())
         {
             state = State.Chasing;
         }
         else if (state == State.Chasing && !enemy.IsPlayerInRange(chaseRange))
         {
             state = State.Patrolling;
         } 

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

        if (Vector2.Distance(enemyPos, pointPos) < 0.2f)
        {
            if (waitTime > 0) { enemy.Movement.StopForFrame(); waitTime -= Time.deltaTime; return; }

            currentPathIndex = GetNextPointIndex(); // Update the current path index with the next index to go to
            waitTime = startWaitTime;
        }

        enemy.Movement.ChangeDirection(pathPoints[currentPathIndex].position);

    }

    private void Chase()
    {
        enemy.Movement.ChangeDirection(Player.Instance.GetPosition());

        if (enemy.IsPlayerInRange(attackRange))
        {
            if (!enemy.Attack.IsAttacking())
            {
                enemy.Animator.SetTrigger("Attack");
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

    public void SetState(State _state)
    {
        state = _state;
    }

    void OnDeath()
    {
        state = State.Dead;
    }

    void OnTakeDamage()
    {
        state = State.Chasing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

    }

}

public enum State
{
    Patrolling,
    Chasing,
    Dead,
}

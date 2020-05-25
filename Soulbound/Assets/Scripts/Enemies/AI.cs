using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    #region STATE VARIABLES
    public StateMachine stateMachine { get; set; }

    public IdleState idleState;
    public PatrolState patrolState;
    public ChaseState chaseState;
    #endregion

    #region VARIABLES
    private Enemy enemy;
    public EnemySight sight;
    public float chaseRange = 7f;
    public EnemyPath path;
    public int startPathIndex = 1;

    public List<float> waitTimes;

    public event Action attackEvent;
    #endregion

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public virtual void Start()
    {
        stateMachine = new StateMachine();

        idleState = new IdleState(this, stateMachine, waitTimes);
        patrolState = new PatrolState(this, stateMachine, path, startPathIndex);
        chaseState = new ChaseState(this, stateMachine, chaseRange);

        chaseState.attackEvent += OnAttack;

        stateMachine.Initialize(patrolState);

        enemy.onDeathEvent += OnDeath;
        enemy.Senses.onTakeDamageEvent += OnTakeDamage;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    void OnDeath()
    {
        Debug.Log("dead");
    }

    void OnTakeDamage()
    {
        stateMachine.ChangeState(chaseState);
    }

    void OnAttack()
    {
        attackEvent();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

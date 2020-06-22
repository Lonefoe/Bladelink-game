using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType { Stationary, Walker, }

public class AI : MonoBehaviour
{
    #region STATE VARIABLES
    public StateMachine stateMachine { get; set; }

    public IdleState idleState;
    public PatrolState patrolState;
    public WanderState wanderState;
    public ChaseState chaseState;
    public DeadState deadState;
    public AttackState attackState;
    #endregion

    public ChaseState_Data chaseState_Data;

    #region VARIABLES
    private Enemy enemy;
    public MovementType movementType = MovementType.Walker;
    public EnemySight sight;

    public EnemyPath path;
    public int startPathIndex = 1;
    public float timeSinceLastAttack { get; set; }

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
        patrolState = new PatrolState(this, stateMachine, movementType, path, startPathIndex);
        wanderState = new WanderState(this, stateMachine);
        chaseState = new ChaseState(this, stateMachine, chaseState_Data);
        deadState = new DeadState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);

        attackState.attackEvent += OnAttack;

        stateMachine.Initialize(patrolState);

        enemy.onDeathEvent += OnDeath;
        enemy.Senses.onTakeDamageEvent += OnTakeDamage;
    }

    private void Update()
    {
        stateMachine.Update();

        timeSinceLastAttack += Time.deltaTime;
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
        timeSinceLastAttack = 0f;
    }
 
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseState_Data.chaseRange);
    }


}

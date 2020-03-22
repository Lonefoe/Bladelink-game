using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{

    Animator animator;
    CharacterController controller;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private List<Transform> patrolPoints;
    private int currentPatrolIndex;
    private int direction = 1; // 1 = facing right, -1 = facing left

    // Reference setup
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentPatrolIndex = patrolPoints.Count - 1;
    }

    void Update()
    {
        if (transform.position.x < patrolPoints[currentPatrolIndex].position.x && direction == -1)
        {
            direction = 1;
            currentPatrolIndex = patrolPoints.Count - 1;
        } else if (transform.position.x > patrolPoints[currentPatrolIndex].position.x && direction == 1)
        {
            direction = -1;
            currentPatrolIndex -= 1;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

    }

    void FixedUpdate()
    {
        if (direction == 1)
        {
            controller.Move(moveSpeed * Time.fixedDeltaTime, false, false);
        } else if (direction == -1)
        {
            controller.Move(-moveSpeed * Time.fixedDeltaTime, false, false);
        }
    }

    // Is called from the combat script
    // Will be replaced with an interface or something that is more scalable than this
    public void TakeDamage (int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();              // When enemy's health drops below zero, he dies
        }
    }

    // Is called when our health drops below zero
    // Will be replaced with an interface or something that is more scalable than this
    void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
    }

}

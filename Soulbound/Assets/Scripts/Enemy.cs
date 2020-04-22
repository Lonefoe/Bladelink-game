using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{
    // All references
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public CharacterController Controller { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemyAI AI { get; private set; }
    public EnemyAttack Attack { get; private set; }


    public EnemyStats Stats = new EnemyStats();
    private int currentHealth;

    public event Action onDeathEvent;

    void Awake ()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Controller = GetComponent<CharacterController>();
        Movement = GetComponent<EnemyMovement>();
        AI = GetComponent<EnemyAI>();
        Attack = GetComponent<EnemyAttack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Stats.maxHealth;

    }


    // Is called from the combat script
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Animator.SetTrigger("Hurt");
        AudioManager.Instance.PlayOneShot("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Is called when our health drops below zero
    public void Die()
    {
        onDeathEvent();
        Animator.SetTrigger("Death");
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

    }

}

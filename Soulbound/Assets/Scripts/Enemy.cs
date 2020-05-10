using System;
using System.Collections;
using UnityEngine;
using Senses;

public class Enemy : MonoBehaviour, IDamageable<int>
{
    // All references
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public CharacterController Controller { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemyAI AI { get; private set; }
    public EnemyAttack Attack { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public SenseManager Senses = new SenseManager();

    public EnemyStats Stats = new EnemyStats();
    private int currentHealth;
    private int currentPoise;
    private bool dead = false;

    public event Action onDeathEvent;

    void Awake ()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Controller = GetComponent<CharacterController>();
        Movement = GetComponent<EnemyMovement>();
        AI = GetComponent<EnemyAI>();
        Attack = GetComponent<EnemyAttack>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Stats.maxHealth;
        currentPoise = Stats.poise;
    }

    
    // Is called from the combat script
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Senses.Report(Sense.Damage);
        StartCoroutine(Flasher(Color.red, Renderer.color));
        Movement.Knockback(580f, true);

        currentPoise -= damage;
        if (!Attack.IsSlashing())
        {
            if (currentPoise <= 0) { currentPoise = Stats.poise; Animator.SetTrigger("Hurt"); }
        }

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
        dead = true;
        Animator.SetTrigger("Death");
        Rigidbody.isKinematic = true;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        Player.currentSoulPoints += UnityEngine.Random.Range(0.5f, 1f);
    }

    public bool IsDead()
    {
        return dead;
    }

    public bool IsPlayerInRange(float range)
    {
        if (Vector2.Distance(transform.position, Player.Instance.GetPosition()) < range) { return true; }
        else { return false; }

    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    IEnumerator Flasher(Color collideColor, Color normalColor)
    {
        for (int i = 0; i < 1; i++)
        {
            Renderer.material.SetFloat("Flash", 1);
            yield return new WaitForSeconds(.1f);
            Renderer.material.SetFloat("Flash", 0);
            yield return new WaitForSeconds(.1f);
        }
    }

}

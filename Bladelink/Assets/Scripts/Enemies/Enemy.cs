using System;
using System.Collections;
using UnityEngine;
using Senses;

public class Enemy : MonoBehaviour, IDamageable<int>
{
    // All references
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public EnemyController Controller { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public AI AI { get; private set; }
    public EnemyAttack Attack { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public SenseManager Senses = new SenseManager();

    public EnemyStats Stats = new EnemyStats();
    public int currentHealth { get; private set;}
    private int currentPoise;
    private bool dead = false;
    public bool hurt { get; set; }

    public event Action onDeathEvent;

    void Awake ()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Controller = GetComponent<EnemyController>();
        Movement = GetComponent<EnemyMovement>();
        AI = GetComponent<AI>();
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
        Movement.Knockback(320f, true);
        
        if (!Attack.IsSlashing())
        {
            if (currentPoise <= 0 && !hurt) { currentPoise = Stats.poise; hurt = true; Animator.SetTrigger("Hurt"); }
        }
        if (!hurt) currentPoise -= damage;

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
        Animator.SetBool("Dead", true);
        transform.Find("AntiPlayerForce").gameObject.SetActive(false);
        
        Rigidbody.isKinematic = true;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        
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

    public void OnParried()
    {
        Animator.SetTrigger("PlayerDeflected");
        hurt = true;
        currentPoise = Stats.poise;
    }

}

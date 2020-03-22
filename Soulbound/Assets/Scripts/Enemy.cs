using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator animator;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Reference setup
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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

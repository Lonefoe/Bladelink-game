using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable<int>
{
    // All static references to all player's components
    public static Animator Animator { get; private set; }
    public static Rigidbody2D Rigidbody { get; private set; }
    public static CharacterController Controller { get; private set; }
    public static Player Instance { get; private set; }
    public static PlayerMovement Movement { get; private set; }
    public static PlayerCombat Combat { get; private set; }

    SpriteRenderer renderer;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private Vector2 startingPosition;

    private void Awake()
    {
        Instance = this;
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        startingPosition = transform.position;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(Flasher(Color.red, renderer.color));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Movement.enabled = false;
        Invoke("Respawn", 1f);
    }

    void Respawn()
    {
        transform.position = startingPosition;
        Movement.enabled = true;
        currentHealth = maxHealth;
    }

    IEnumerator Flasher(Color collideColor, Color normalColor)
    { 
        for (int i = 0; i < 3; i++)
        {
            renderer.color = collideColor;
            yield return new WaitForSeconds(.1f);
            renderer.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
    }

}

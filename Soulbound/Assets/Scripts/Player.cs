using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable<int>
{
    // All static references to all player's components
    public static Animator Anim { get; private set; }
    public static Rigidbody2D Rigidbody { get; private set; }
    public static PlayerController Controller { get; private set; }
    public static Player Instance { get; private set; }
    public static PlayerMovement Movement { get; private set; }
    public static PlayerCombat Combat { get; private set; }
    public static SpriteRenderer Renderer { get; private set; }

    [SerializeField] PlayerStats Stats = new PlayerStats();    
    public static int currentHealth;
    public static int currentPoise;
    public static float currentSoulPoints;
    private Vector2 startingPosition;

    private void Awake()
    {
        Instance = this;
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Controller = GetComponent<PlayerController>();
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = Stats.maxHealth;
        currentPoise = Stats.maxPoise;
        currentSoulPoints = Stats.maxSoulPoints;
        startingPosition = transform.position;
    }

    void Update()
    {
        UIManager.Instance.healthSlider.value = currentHealth;
        UIManager.Instance.soulPointsText.text = currentSoulPoints.ToString("f2");
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(Flasher(Color.clear, Renderer.color));
        AudioManager.Instance.PlayOneShot("Hit");
        Movement.Knockback(580f);

        if (currentPoise <= 0) { currentPoise = Stats.maxPoise; Anim.SetBool("Hurt", true); }
        else currentPoise -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Movement.DisableMovement();
        Invoke("Respawn", 1f);
    }

    void Respawn()
    {
        transform.position = startingPosition;
        Movement.EnableMovement();
        currentHealth = Stats.maxHealth;
    }

    IEnumerator Flasher(Color collideColor, Color normalColor)
    { 
        for (int i = 0; i < 3; i++)
        {
            Renderer.color = collideColor;
            yield return new WaitForSeconds(.1f);
            Renderer.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void PlayFootstep()
    {
        int num = Random.Range(1, 4);

        switch (num)
        {
            case 1:
              AudioManager.Instance.Play("Footstep01");
                break;

            case 2:
                AudioManager.Instance.Play("Footstep02");
                break;

            case 3:
                AudioManager.Instance.Play("Footstep03");
                break;
        }
    }

}
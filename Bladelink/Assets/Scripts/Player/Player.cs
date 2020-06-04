using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable<int>
{
    #region VARIABLES
    // All static references to all player's components
    public static Animator Anim { get; private set; }
    public static Rigidbody2D Rigidbody { get; private set; }
    public static PlayerController Controller { get; private set; }
    public static Player Instance { get; private set; }
    public static PlayerMovement Movement { get; private set; }
    public static PlayerCombat Combat { get; private set; }
    public static SpriteRenderer Renderer { get; private set; }

    [Header("Health")]
    [SerializeField] Health health = new Health();

    [Header("Stats")]
    public PlayerStats Stats = new PlayerStats();    
    public static int currentHealth;
    public static int currentPoise;
    public static float currentSoulPoints;
    private Vector2 savedPos;
    private bool isDead = false;
    private bool flashing;

    #endregion

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
        savedPos = transform.position;
    }

    void Update()
    {
        health.UpdateHeartUI();
        UIManager.Instance.soulPointsText.text = currentSoulPoints.ToString("f2");
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (!flashing) StartCoroutine(Flasher(Color.clear, Renderer.color));
        AudioManager.Instance.PlayOneShot("Hit");

        if (currentPoise <= 0) { currentPoise = Stats.maxPoise; Anim.SetBool("Hurt", true); }
        else currentPoise -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Anim.SetBool("Dead", true);
        Player.DisableControl(true);
        Controller.FreezePosition(true, true);
        isDead = true;
        Invoke("Respawn", 4f);
    }

    void Respawn()
    {
        transform.position = savedPos;
        Player.DisableControl(false);
        Controller.FreezePosition(false);
        isDead = false;
        Player.Combat.ResetCombo();
        currentHealth = Stats.maxHealth;
        Anim.SetBool("Dead", false);
    }

    IEnumerator Flasher(Color collideColor, Color normalColor)
    {
        flashing = true;
        for (int i = 0; i < 3; i++)
        {
            Renderer.color = collideColor;
            yield return new WaitForSeconds(.1f);
            Renderer.color = normalColor;
            yield return new WaitForSeconds(.1f);
        }

        flashing = false;
    }

    void HurtEnd()
    {
        Anim.SetBool("Hurt", false);
    }

    public static void AddRandomSoulPoints(float min = 0.1f, float max = 0.3f)
    {
        currentSoulPoints += UnityEngine.Random.Range(min, max);
    }

    private void PlayFootstep()
    {
        AudioManager.Instance.PlayOneShot("PlayerFootstep");
    }

    public void SetSavePos(Vector2 value)
    {
        savedPos = value;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public static void DisableControl(bool disable)
    {
        if (disable)
        {
        Combat.enabled = false;
        Movement.DisableMovement();
        Controller.enabled = false;
        }
        else
        {
        Combat.enabled = true;
        Movement.EnableMovement();
        Controller.enabled = true;  
        }
    }
}
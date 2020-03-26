using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<int>
{
    Animator animator;
    public EnemyMovement Movement { get; set; }
   
    public EnemyStats stats = new EnemyStats();

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 35;
    private int currentHealth;


    void Awake ()
    {
        animator = GetComponent<Animator>();
        Movement = GetComponent<EnemyMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        stats.Initiate(maxHealth, damage) ;

    }


    // Is called from the combat script
    // Will be replaced with an interface or something that is more scalable than this
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
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

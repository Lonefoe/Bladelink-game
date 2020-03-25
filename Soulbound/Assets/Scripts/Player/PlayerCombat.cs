using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageable<int>
{
    // Cached references
    Animator animator;
    PlayerMovement movement;
    CharacterController controller;

    // Properties
    public int attackDamage = 20;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    private bool chainAttack = true;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    private void Awake()
    {
        // Setting up references
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (chainAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetInteger("ComboPoint", comboPoint);
                animator.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
                chainAttack = false;                            // Can't attack until we've hit the slash frame of the animation
            }
        }

    }

    // All of the attack functionality
    // Triggered by an animation event
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        comboPoint++;               // Add a combo point
        movement.enabled = false;   // Disable movement script
        if (hitEnemies.Length <= 0) controller.Launch(launchForce);    // Launch character forward a bit
        chainAttack = true;         // Chain the next attack

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage
        }
    }

    public void TakeDamage(int damage)
    {
        // Take damage
    }

    // Triggered by an animation event
    public void ResetCombo()
    {
        comboPoint = 0;          // Reset combo points
        movement.enabled = true; // Enable movement script
        chainAttack = true;      // Make sure we can start a new attack
    }

    // Gizmos in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

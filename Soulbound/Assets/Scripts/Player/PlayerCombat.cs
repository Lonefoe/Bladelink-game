using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 20;
    public float throwForce = 10f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    private bool chainAttack = true;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    [SerializeField] GameObject swordPrefab;
    private GameObject mySword;
    private bool canReturn = false;

    public bool CanReturn
    {
        get
        {
            return canReturn;
        }
        set
        {
            canReturn = value;
        }
    }

    private void Update()
    {
        // ATTACK
        if (InputManager.Instance.attackPressed && mySword == null)
        {
            if (chainAttack)
            {
                Player.Animator.SetInteger("ComboPoint", comboPoint);
                Player.Animator.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
                chainAttack = false;                            // Can't attack until we've hit the slash frame of the animation
                AudioManager.Instance.PlayOneShot("Slash");
            }
        }

        if (InputManager.Instance.throwPressed)
        {
             SwordThrow();

        }

    }

    // All of the attack functionality
    // Triggered by an animation event
    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        comboPoint++;               // Add a combo point
        Player.Movement.enabled = false;   // Disable movement script
        Player.Rigidbody.Sleep();
        if (hitEnemies.Length <= 0) Player.Controller.Launch(launchForce);    // Launch character forward a bit
        chainAttack = true;         // Chain the next attack

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage

            var enemyRB = enemy.GetComponent<Rigidbody2D>();
            int dir = enemy.GetComponent<EnemyMovement>().GetDirection();

            enemyRB.velocity = new Vector2(enemyRB.velocity.x * -1, 0f);         
        }
    }

    // Triggered by an animation event
    public void ResetCombo()
    {
        comboPoint = 0;          // Reset combo points
        Player.Movement.enabled = true; // Enable movement script
        chainAttack = true;      // Make sure we can start a new attack
    }


    private void SwordThrow()
    {
        if (mySword == null && Sword.Instance == null)
        {
            mySword = Instantiate(swordPrefab, attackPoint.position, swordPrefab.transform.rotation) as GameObject;
            canReturn = false;

            // Throw the sword
            if (Player.Controller.IsFacingRight()) mySword.GetComponent<Sword>().Throw(transform.right * throwForce);
            else mySword.GetComponent<Sword>().Throw(-transform.right * throwForce);
        }
        else if (canReturn == true)
        {
            mySword.GetComponent<Sword>().Return();
        }
    }

    // Gizmos in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

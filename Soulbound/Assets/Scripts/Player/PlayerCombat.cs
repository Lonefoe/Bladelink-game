using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 20;
    public float throwForce = 10f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    private bool deflecting;
    [SerializeField] private LayerMask enemyLayer;

    private bool chainAttack = true;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    [SerializeField] GameObject swordPrefab;
    [SerializeField] float swordThrowCost = 1;
    private GameObject mySword;
    private bool canReturn = false;

    private Shield shield;

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

    private void Awake()
    {
        InputManager.controls.Player.Attack.performed += ctx => HandleAttack();
        InputManager.controls.Player.Throw.performed += ctx => SwordThrow();
        InputManager.controls.Player.Deflect.started += ctx => Deflect();
        InputManager.controls.Player.Deflect.canceled += ctx => StopDeflect();
    }

    private void Start()
    {
        shield = GetComponentInChildren<Shield>();
    }

    private void HandleAttack()
    {
        if (chainAttack && mySword == null)
        {
            Player.Animator.SetInteger("ComboPoint", comboPoint);
            Player.Animator.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
            chainAttack = false;                            // Can't attack until we've hit the slash frame of the animation
            AudioManager.Instance.PlayOneShot("Slash");
        }
    }

    // All of the attack functionality
    // Triggered by an animation event
    public void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        comboPoint++;               // Add a combo point
        Player.Movement.enabled = false;   // Disable movement script
        Player.Rigidbody.Sleep();
        if (hitColliders.Length <= 0) Player.Controller.Launch(launchForce);    // Launch character forward a bit
        chainAttack = true;         // Chain the next attack

        List<GameObject> hitEnemies = new List<GameObject>();

        foreach (Collider2D enemy in hitColliders)
        {
            if (hitEnemies.Contains(enemy.gameObject)) return;
            hitEnemies.Add(enemy.gameObject);
            HitEnemy(enemy);
        }
    }

    public void HitEnemy(Collider2D enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage
        CameraEffects.Instance.Shake(0.08f, 2.2f);
        StartCoroutine(CameraEffects.Instance.PauseEffect(.1f));
    }

    public void Deflect()
    {
        deflecting = true;
        Player.Animator.SetBool("deflecting", deflecting);
    //    shield.Show();
    }

    public void StopDeflect()
    {
        deflecting = false;
        Player.Animator.SetBool("deflecting", deflecting);
    //    shield.Hide();
    }

    public bool IsDeflecting() { return deflecting; }

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
            if (Player.currentSoulPoints < swordThrowCost) return;
            Player.currentSoulPoints -= swordThrowCost;

            mySword = Instantiate(swordPrefab, attackPoint.position, swordPrefab.transform.rotation) as GameObject;
            Player.Animator.SetTrigger("throw");
            canReturn = false;

            // Decide where to throw the sword and then throw it
            if (Player.Controller.IsFacingRight()) mySword.GetComponent<Sword>().Throw(transform.right * throwForce);
            else mySword.GetComponent<Sword>().Throw(-transform.right * throwForce);
        }
        else if (canReturn == true)
        {
            Player.Controller.Face(mySword);
            mySword.GetComponent<Sword>().Return();
            Player.Animator.SetTrigger("throw");
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

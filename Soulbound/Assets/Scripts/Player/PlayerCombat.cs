using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attacking")]
    public int attackDamage = 20;
    public float throwForce = 10f;
    public CombatMode Mode = new CombatMode();
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    private bool attacking;
    private bool deflecting;
    [SerializeField] private LayerMask enemyLayer;

    private bool chainAttack = true;
    private bool rememberChain = false;
    private bool canDeflect = true;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    [Header("Sword")]
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
        InputManager.controls.Player.Deflect.started += ctx => StartDeflect();
        InputManager.controls.Player.Deflect.canceled += ctx => StopDeflect();
    }

    private void Start()
    {
        shield = GetComponentInChildren<Shield>();
    }

    //=====================================================
    // Attack Input (Called when player presses button)
    //=====================================================  
    private void HandleAttack()
    {
        if (chainAttack && !deflecting && mySword == null)
        {
            Player.Anim.SetInteger("ComboPoint", comboPoint);
            Player.Anim.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
            attacking = true;
            chainAttack = false;                            // Can't attack until we've hit the slash frame of the animation
            canDeflect = false;
            AudioManager.Instance.PlayOneShot("Slash");
        }
        else if (!chainAttack && mySword == null) rememberChain = true;
        else if (mySword != null) PortToSword(); 
    }

    //=====================================================
    // Attack (Called by an animation event)
    //=====================================================  
    public void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        comboPoint++;               // Add a combo point   
        Player.Movement.DisableMovement();   // Disable movement script
        Player.Rigidbody.Sleep();

        if (hitColliders.Length <= 0) Player.Controller.Launch(launchForce);    // Launch character forward a bit

        List<GameObject> hitEnemies = new List<GameObject>();
        foreach (Collider2D enemy in hitColliders)
        {
            if (hitEnemies.Contains(enemy.gameObject)) return;
            hitEnemies.Add(enemy.gameObject);
            HitEnemy(enemy);
        }
    }

    //=====================================================
    // Deflect (Called when two swords meet)
    //=====================================================  
    public void Deflect()
    {
        CameraEffects.Instance.Shake(0.05f, 2.4f);
        StartCoroutine(CameraEffects.Instance.PauseEffect(.1f));
        AudioManager.Instance.PlayOneShot("Deflect");
        EffectsManager.Instance.SpawnParticles("Deflect", shield.transform.position);
        Player.Movement.Knockback(400f);
    }

    //=====================================================
    // Throwing the sword ability
    //=====================================================  
    private void SwordThrow()
    {
        if (mySword == null && PlayerSword.Instance == null)
        {
            if (Player.currentSoulPoints < swordThrowCost || attacking) return;
            Player.currentSoulPoints -= swordThrowCost;
            Player.Anim.SetTrigger("throw");

        }
        else if (canReturn == true)
        {
            Player.Controller.Face(mySword);
            mySword.GetComponent<PlayerSword>().Return();
            Player.Anim.SetTrigger("throw");
        }

    }

    // Triggered by an animation event
    private void SpawnSword()
    {
        if (mySword != null || PlayerSword.Instance != null) return;

        // Spawn the sword
        mySword = Instantiate(swordPrefab, attackPoint.position, swordPrefab.transform.rotation) as GameObject;
        canReturn = false;

        // Decide where to throw the sword and then throw it
        if (Player.Controller.IsFacingRight()) mySword.GetComponent<PlayerSword>().Throw(transform.right * throwForce);
        else mySword.GetComponent<PlayerSword>().Throw(-transform.right * throwForce);
    }

    // Second part of the ability - player ports to the sword and absorbs it
    private void PortToSword()
    {
        transform.position = mySword.transform.position + new Vector3(0f, 0.5f, 0f);
        Destroy(mySword.gameObject);
    }

    //=====================================================
    // Things that happen when enemy's hit
    //=====================================================  
    public void HitEnemy(Collider2D enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage
        CameraEffects.Instance.Shake(0.08f, 2.2f);
        StartCoroutine(CameraEffects.Instance.PauseEffect(.1f));
        
    }

    //=====================================================
    // Deflecting Input
    //=====================================================  
    public void StartDeflect()
    {
        if (!canDeflect) { deflecting = true; return; }

        deflecting = true;
        Player.Anim.SetBool("deflecting", deflecting);
        shield.Show();
        ResetCombo();
    }

    public void StopDeflect()
    {
        deflecting = false;
        Player.Anim.SetBool("deflecting", deflecting);
        shield.Hide();
    }

    public bool IsDeflecting() { return shield.IsShielding(); }

    //=====================================================
    // Resetting the combo - player will start from attack 1
    //=====================================================  
    public void ResetCombo()
    {
        comboPoint = 0;          // Reset combo points
        Player.Movement.EnableMovement(); // Enable movement script
        chainAttack = true;      // Make sure we can start a new attack 01
        attacking = false;
    }

    //=====================================================
    // Chaining next attack
    //=====================================================  
    public void Chain()
    {
        chainAttack = true;         // Chain the next attack
        canDeflect = true;
        if (deflecting) StartDeflect();
        if (rememberChain) { HandleAttack(); rememberChain = false; }
    }

    // Gizmos in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

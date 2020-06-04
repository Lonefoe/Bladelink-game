using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCombat : MonoBehaviour
{
    #region VARIABLES

    [Header("Attacking")]
    public int attackDamage = 20;
    public float throwForce = 10f;
    public CombatMode Mode = new CombatMode();
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    private bool attacking, deflecting, rememberDeflect;
    [SerializeField] private LayerMask enemyLayer, grassLayer;
    public CinemachineVirtualCamera combatCam;

    private bool chainAttack = true, rememberChain = false;
    private bool canDeflect = true, canAttack = true;
    private float parryTime; 
    public float startParryTime = 0.15f;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    [Header("Sword")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float swordThrowCost = 1;
    private GameObject mySword;
    private bool canReturn = false;

    private Shield shield;

    #endregion

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

    private void Update()
    {
        if (deflecting)
        {
            parryTime -= Time.deltaTime;
        }
        else parryTime = startParryTime;
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
        if (chainAttack && mySword == null && canAttack)
        {
            if (!CanAttack()) return;
            Player.Anim.SetInteger("ComboPoint", comboPoint);
            Player.Anim.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
            attacking = true;
            chainAttack = false;                            // Can't attack until we've hit the slash frame of the animation
            canDeflect = false;
            AudioManager.Instance.PlayOneShot("Slash");
        }
        else if (!chainAttack && mySword == null && canAttack) rememberChain = true;
        else if (mySword != null) StartCoroutine(PortToSword());
    }

    #region ATTACK
    //=====================================================
    // Attack (Called by an animation event)
    //=====================================================  
    public void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);      

        comboPoint++;               // Add a combo point   
        Player.Movement.DisableMovement();   // Disable movement script
        Player.Rigidbody.Sleep();

        if (hitColliders.Length <= 0 && IsEnemyBehindPlayer()) { Player.Controller.Flip(); hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer); }
        if (hitColliders.Length <= 0) Player.Controller.Launch(launchForce);    // Launch character forward a bit

        List<GameObject> hitEnemies = new List<GameObject>();
        foreach (Collider2D enemy in hitColliders)
        {
            if (hitEnemies.Contains(enemy.gameObject)) return;
            hitEnemies.Add(enemy.gameObject);
            HitEnemy(enemy);
        }
    }

    // We check if enemy's behind the player, if it is, we flip him = helper
    private bool IsEnemyBehindPlayer()
    {
        Collider2D[] safetyHit = Physics2D.OverlapCircleAll(new Vector2(attackPoint.position.x - attackPoint.localPosition.x * 2 * Player.Movement.GetDirection(), attackPoint.position.y), attackRange + 0.07f, enemyLayer);
        if (safetyHit.Length > 0 && comboPoint == 1) return true;
        else return false;
    }

    #endregion

    #region DEFLECT
    //=====================================================
    // Deflecting Input
    //=====================================================  
    public void StartDeflect()
    {
        if (!canDeflect) { rememberDeflect = true; return; }
        else rememberDeflect = false;
        if (!CanDeflect()) return;

        deflecting = true;
        Player.Controller.SetShouldFlip(false);
        Player.Movement.SetMoveMultiplier(0.3f);
        Player.Anim.SetBool("deflecting", deflecting);
        shield.Show();
        ResetCombo();
    }

    public void StopDeflect()
    {
        deflecting = false;
        Player.Controller.SetShouldFlip(true);
        Player.Movement.SetMoveMultiplier(1f);
        Player.Anim.SetBool("deflecting", deflecting);
        shield.Hide();
    }

    //=====================================================
    // Deflect (Called when two swords meet)
    //=====================================================  
    public void Deflect(Enemy enemy)
    {
        CameraEffects.Instance.Shake(0.05f, 2.4f);
        AudioManager.Instance.PlayOneShot("Deflect");
        InputManager.Instance.Vibrate(0.18f, 0.28f, 0.3f);
        EffectsManager.Instance.SpawnParticles("Deflect", shield.transform.position);
        Player.Movement.Knockback(400f);

        // If we successfully parried, do this
        if (parryTime > 0)
        {
            enemy.OnParried();
            StartCoroutine(CameraEffects.Instance.Slowmotion(0.2f, 0.5f));
            parryTime = startParryTime;
        }
    }

    #endregion

    #region SWORD THROW

    //=====================================================
    // Throwing the sword ability
    //=====================================================  
    private void SwordThrow()
    {
        if (mySword == null && PlayerSword.Instance == null)
        {
            if (!CanThrowSword()) return;
            if (Player.currentSoulPoints < swordThrowCost || attacking) return;

            Player.currentSoulPoints -= swordThrowCost;
            Player.Anim.SetTrigger("throw");
            StartCoroutine(CameraEffects.Instance.Slowmotion(.4f, 0.2f));
            AudioManager.Instance.Play("SwordSwoosh");

            Player.Movement.Knockback(150f);
        }
        else if (canReturn == true)
        {
            if (!CanReturnSword()) return;

            Player.Controller.Face(mySword);
            mySword.GetComponent<PlayerSword>().Return();
            Player.Anim.SetTrigger("throw");
            AudioManager.Instance.Play("SwordSwoosh");
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
    private IEnumerator<WaitForSecondsRealtime> PortToSword()
    {
        AudioManager.Instance.Play("SwordPort");
        StartCoroutine(CameraEffects.Instance.Slowmotion());
        yield return new WaitForSecondsRealtime(1f);
        transform.position = mySword.transform.position + new Vector3(0f, 0.5f, 0f);
        Destroy(mySword.gameObject);
    }

    #endregion

    //=====================================================
    // Things that happen when enemy's hit
    //=====================================================  
    public void HitEnemy(Collider2D enemy, bool pause = true)
    {
        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage
        Player.AddRandomSoulPoints(0.1f, 0.3f);
        CameraEffects.Instance.Shake(0.12f, 2.2f);
        InputManager.Instance.Vibrate(0.12f, 0.25f, 0.4f);
        if(pause) StartCoroutine(CameraEffects.Instance.PauseEffect(0.13f));
        EffectsManager.Instance.SpawnParticles("CircleHit", enemy.transform.position + new Vector3(Random.Range(0f, 0.25f), Random.Range(0.5f, 0.75f), 0), true);
        Player.Movement.Knockback(38f);
    }

    //=====================================================
    // Resetting the combo - player will start from attack 1
    //=====================================================  
    public void ResetCombo()
    {
        comboPoint = 0;          // Reset combo points
        Player.Movement.EnableMovement(); // Enable movement script
        chainAttack = true;      // Make sure we can start a new attack 01
        rememberChain = false;
        attacking = false;
        canDeflect = true;
    }

    //=====================================================
    // Chaining next attack
    //=====================================================  
    public void Chain()
    {
        chainAttack = true;         // Chain the next attack
        canDeflect = true;
        if (rememberDeflect) StartDeflect();
        if (rememberChain) { HandleAttack(); rememberChain = false; }
    }

    public bool IsDeflecting() { return shield.IsShielding(); }

    public bool IsAttacking() { return attacking; }

    private bool CanAttack()
    {
        if (Player.Controller.IsClimbingLedge() || Player.Instance.IsDead() || deflecting || mySword != null || !canAttack) return false;
        else return true;
    }
    private bool CanDeflect()
    {
            if (Player.Controller.IsClimbingLedge() || Player.Instance.IsDead() || deflecting || mySword != null) return false;
            else return true;
    }
    private bool CanThrowSword()
    {
        if (Player.Controller.IsClimbingLedge() || Player.Instance.IsDead() || deflecting || mySword != null || attacking) return false;
        else return true;
    }
    private bool CanReturnSword()
    {
            if (Player.Controller.IsClimbingLedge() || Player.Instance.IsDead()) return false;
            else return true;
    }

    // Gizmos in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.red;
    }

}

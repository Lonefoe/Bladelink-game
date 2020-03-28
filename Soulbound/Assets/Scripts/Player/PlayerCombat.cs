﻿using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 20;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    private bool chainAttack = true;
    private int comboPoint = 0;
    [SerializeField] private float launchForce = 125f;

    void Update()
    {
        if (chainAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Player.Animator.SetInteger("ComboPoint", comboPoint);
                Player.Animator.SetTrigger("Attack");                  // Trigger attack animation with a specific combo point
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
        Player.Movement.enabled = false;   // Disable movement script
        Player.Rigidbody.Sleep();
        if (hitEnemies.Length <= 0) Player.Controller.Launch(launchForce);    // Launch character forward a bit
        chainAttack = true;         // Chain the next attack

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);   // We call for the enemy to take damage
        }
    }


    // Triggered by an animation event
    public void ResetCombo()
    {
        comboPoint = 0;          // Reset combo points
        Player.Movement.enabled = true; // Enable movement script
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

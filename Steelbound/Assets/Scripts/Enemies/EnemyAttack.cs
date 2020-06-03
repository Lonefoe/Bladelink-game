﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemy;

    public Transform attackPoint;
    public float attackRange = 0.8f;
    [SerializeField] private LayerMask playerLayer;
    private bool attacking = false, slashing = false;

    public event Action onSlashEvent;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();  
    }

    private void Start()
    {
        enemy.AI.attackEvent += OnAttackStart;
    }

    // Called by an animation event
    public virtual void Attack()
    {   
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, 0.6f, playerLayer);
        AudioManager.Instance.PlayOneShot("Slash");
        slashing = true;
        onSlashEvent();

        if (hitPlayer != null && hitPlayer.gameObject.CompareTag("Player"))
        {
            // If player's deflecting and facing this way
            if (Player.Combat.IsDeflecting() && Utils.AreCharactersFacing(Player.Controller, enemy.Controller)) 
            {               
                Player.Combat.Deflect(enemy);
                enemy.Movement.Knockback(-100f);
                return; 
            }
            Player.Instance.TakeDamage(enemy.Stats.damage);
            Player.Movement.Knockback(-580f * enemy.Movement.GetDirection());
            AudioManager.Instance.PlayOneShot("Hit");
        }

    }

    public void ResetAttack()
    {
        attacking = false;
        slashing = false;
        enemy.Movement.EnableMovement();
    }

    public void OnAttackStart()
    {
        attacking = true;

        float randomSpeed = UnityEngine.Random.Range(-0.05f, 0.5f);
        double rSpeed = System.Math.Round(randomSpeed, 2);
        enemy.Animator.SetFloat("GameSpeed", enemy.Animator.GetFloat("GameSpeed") + (float)rSpeed);     
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    public bool IsSlashing()
    {
        return slashing;
    }

}
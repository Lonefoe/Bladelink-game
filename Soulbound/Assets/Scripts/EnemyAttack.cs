using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemy;

    public Transform attackPoint;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private LayerMask playerLayer;
    private bool attacking = false;

    public event Action onAttackEvent;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    // Called by an animation event
    public virtual void Attack()
    {
        attacking = true;
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, 0.5f, playerLayer);
        onAttackEvent();

        if (hitPlayer != null && hitPlayer.gameObject == Player.Instance.gameObject)
        {
            if (Player.Combat.IsDeflecting() && Utils.AreCharactersFacing(Player.Controller, enemy.Controller)) return;
            Player.Instance.TakeDamage(enemy.Stats.damage);
        }

    }

    public void ResetAttack()
    {
        attacking = false;
        enemy.Movement.EnableMovement();
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

}

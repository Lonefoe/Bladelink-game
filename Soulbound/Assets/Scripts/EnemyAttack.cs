using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private LayerMask playerLayer;
    private bool attacking = false;

    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }
    

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }


    public virtual void Attack()
    {
        attacking = true;
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange / 2, playerLayer);
        enemy.Movement.MoveSpeedMultiplier = 0f;

        if (hitPlayer != null && hitPlayer.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.TakeDamage(enemy.Stats.damage);
        }

    }

    public void ResetAttack()
    {
        attacking = false;
        enemy.Movement.MoveSpeedMultiplier = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}

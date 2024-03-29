﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemySight : MonoBehaviour
{
    private Enemy enemy;
    private Transform eyes;

    [Tooltip("If you use rays, detecting will work as a normal human sight, if you don't it will work as a radius")]
    public bool useRays;

    [Tooltip("Number of rays above and under the middle ray")]
    public int rayAmount = 1;
    public float fieldOfViewAngle = 22f;
    public float viewDistance = 6f;

    private bool playerInSight;
    private Vector3 personalLastSighting;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        eyes = GetComponent<Transform>();
    }

    // Checks whether we see the player and returns true / false
    // Uses an x amount of rays to check if the player's in sight
    public bool CanSeePlayer()
    {
        if(useRays)
        {
        float viewDist = viewDistance * (float)enemy.Movement.GetDirection();
        float angle = 0f;
        playerInSight = false;

        for (int i = -rayAmount; i < rayAmount + 1; i++)
        {
            float multiplier = ((float)i) / ((float)rayAmount);
            angle = fieldOfViewAngle * multiplier;

            RaycastHit2D hit = Physics2D.Raycast(eyes.position, Utils.GetVectorFromAngle(angle) * viewDist, viewDistance, 1 << LayerMask.NameToLayer("Player"));

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == Player.Instance.gameObject)
                {
                    playerInSight = true;
                }
            }

            Debug.DrawRay(eyes.position, Utils.GetVectorFromAngle(angle) * viewDist);

        }

        return playerInSight;

        }
        else 
        {
            return IsInRange(viewDistance);
        }
    }
    private bool IsInRange(float range)
    {
        if (Vector2.Distance(enemy.transform.position, Player.Instance.GetPosition()) < range) { return true; }
        else { return false; }

    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.DrawRay(GetComponent<Transform>().position, Utils.GetVectorFromAngle(fieldOfViewAngle) * viewDistance);
            Gizmos.DrawRay(GetComponent<Transform>().position, Utils.GetVectorFromAngle(-fieldOfViewAngle) * viewDistance);
        }
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

}

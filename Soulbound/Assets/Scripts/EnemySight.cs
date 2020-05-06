using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemySight : MonoBehaviour
{
    private Enemy enemy;
    private Transform eyes;

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

    public bool CanSeePlayer()
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

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.DrawRay(GetComponent<Transform>().position, Utils.GetVectorFromAngle(fieldOfViewAngle) * viewDistance);
            Gizmos.DrawRay(GetComponent<Transform>().position, Utils.GetVectorFromAngle(-fieldOfViewAngle) * viewDistance);
        }
    }

}

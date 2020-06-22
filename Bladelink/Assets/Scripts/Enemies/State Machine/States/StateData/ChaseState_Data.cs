using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Data", menuName="ScriptableObjects/State Data/Chase State Data")]
public class ChaseState_Data : ScriptableObject
{
    public float chaseRange = 7f;
    public float minAttackDelay = 1f, maxAttackDelay = 2f;
    public LayerMask whatIsEnemy, whatIsPlayer;
}

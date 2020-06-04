using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public Vector2 ledgeClimbOffset = Vector2.zero;

    public Vector2 UpdateLedgeOffset()
    {
        Debug.Log("update ledge");
        return ledgeClimbOffset;
    }

}

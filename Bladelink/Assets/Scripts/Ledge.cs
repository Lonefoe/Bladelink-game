using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public Transform ledgeGrabPoint;

    public Vector2 UpdateLedgePos()
    {
        Debug.Log("update ledge");
        return ledgeGrabPoint.position;
    }

}

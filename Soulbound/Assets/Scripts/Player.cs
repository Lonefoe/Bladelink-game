using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private void Start()
    {
        Instance = this;  

    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

}

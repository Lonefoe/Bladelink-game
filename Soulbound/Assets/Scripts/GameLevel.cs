using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public static GameLevel Instance;

    public GameObject virtualCamera;

    void Awake()
    {
        Instance = this;
    }

}

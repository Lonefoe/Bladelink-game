using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    private List<Transform> pathPoints = new List<Transform>();

    private void Awake()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child != gameObject.transform)    // We omit the child that is this gameObject's transform
            {
                pathPoints.Add(child);
            }

        }

    }

    public List<Transform> GetPoints()
    {
        return pathPoints;
    }

}

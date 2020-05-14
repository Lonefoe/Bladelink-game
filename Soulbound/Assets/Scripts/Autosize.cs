using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosize : MonoBehaviour
{
    private float startingDistance;
    public Vector3 startingScale;

    void Start()
    {
        //Get starting distance to scale objects by, this is the control.
        startingDistance = Vector3.Distance(Camera.main.transform.position, Vector3.zero);
        //Get starting scale of the object, in the previous version it would have scaled everything to one.
    }

    void Update()
    {
        //Figure out the current distance by finding the difference from starting distance
        float curDistance = Vector3.Distance(Camera.main.transform.position, transform.position) - startingDistance;
        // or was it the other way around, this code is untested!

        //Scale this object depending on distance away to the starting distance
        transform.localScale = ( startingScale * curDistance ) / 10;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxLayer : MonoBehaviour
{ 
    private Vector3 startPos;

    [Range(0,1)] public float ySpeedMultiplier = 1f;
    public GameObject parallaxCam;

    private void Start()
    {
        startPos = transform.position;

    }

    void LateUpdate()
    {
        float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(parallaxCam.transform.position.z / transform.position.z));
        float distanceX = parallaxCam.transform.position.x * parallaxSpeed;
        float distanceY = parallaxCam.transform.position.y * parallaxSpeed * ySpeedMultiplier;

        transform.position = new Vector3(startPos.x + distanceX, startPos.y + distanceY, transform.position.z);
    }

}

﻿using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] float multiplier = 0.0f;
    [SerializeField] bool horizontalOnly = true;
    [SerializeField] float verticalMultiplier = 1f;

    private Transform cameraTransform;

    private Vector3 startCameraPos;
    private Vector3 startPos;

    void Start()
    {
        cameraTransform = GameLevel.Instance.mainVirtualCam.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }


    private void LateUpdate()
    {
        var position = startPos;
        if (horizontalOnly)
            position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
        else
            position += multiplier * (cameraTransform.position - startCameraPos);

        transform.position = position;
    }

}

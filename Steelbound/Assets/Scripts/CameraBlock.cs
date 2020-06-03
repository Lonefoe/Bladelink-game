using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBlock : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == virtualCam)
        {
            Debug.Log("block");
            virtualCam.Follow = virtualCam.transform;
        }
    }
}

using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] float multiplier = 0.0f;
[Range(0,1)][SerializeField] float verticalMultiplier = 1f;
    [SerializeField] bool horizontalOnly = true;

    private Transform cameraTransform;

    private Vector3 startCameraPos;
    private Vector3 startPos;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }


    private void LateUpdate()
    {
        var position = startPos;
        if (horizontalOnly)
            position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
        else
        {
            position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
            position.y += verticalMultiplier * multiplier * (cameraTransform.position.y - startCameraPos.y);
        }

        transform.position = position;
    }

}

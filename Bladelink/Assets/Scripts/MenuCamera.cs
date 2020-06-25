using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float maxSpeed;
    public float xClamp;
    public float yClamp;

    Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x == Screen.width - 1 || Input.mousePosition.y == Screen.height - 1) return;

        Vector3 velocity = Vector3.zero;
        Vector3 pos = transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        pos = new Vector3 (mousePos.x, mousePos.y, cam.transform.position.z);
        pos.x = Mathf.Clamp(pos.x, -xClamp, xClamp);
        pos.y = Mathf.Clamp(pos.y, -yClamp, yClamp);
        transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * maxSpeed);
    }

}

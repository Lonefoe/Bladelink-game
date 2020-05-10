using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 5f;

    private void Update()
    {
        float scrollTo = transform.position.x + scrollSpeed * Time.deltaTime;

        transform.position = new Vector2(scrollTo, transform.position.y);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("invisible");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 5f;
    [SerializeField] private float desiredPos = 7.6f;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.localPosition;
        desiredPos *= scrollSpeed / Mathf.Abs(scrollSpeed);
    }

    private void Update()
    {
        float scrollTo = transform.position.x + scrollSpeed * Time.deltaTime;

        transform.position = new Vector2(scrollTo, transform.position.y);

        if (desiredPos < 0 && transform.localPosition.x < startPos.x + desiredPos || desiredPos > 0 && transform.localPosition.x >= startPos.x + desiredPos)
        {
           StartCoroutine(Fade());
        }

    }

    IEnumerator Fade()
    {
        if (GetComponent<Animation>().isPlaying) yield break;
        GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1);
        transform.localPosition = startPos;
    }

}

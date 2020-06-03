using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    public float time;
    public bool isRealtime = false;

    private void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        if (!isRealtime)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
        else
        {
            yield return new WaitForSecondsRealtime(time);
            Destroy(gameObject);
        }
    }
}

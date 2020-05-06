using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public void Hide()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void Show()
    {
        GetComponent<Collider2D>().enabled = true;
    }

}
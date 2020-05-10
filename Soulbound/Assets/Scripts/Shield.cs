using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void Hide()
    {
        col.enabled = false;
    }

    public void Show()
    {
        col.enabled = true;
    }

    public bool IsShielding()
    {
        return col.enabled;
    }

}
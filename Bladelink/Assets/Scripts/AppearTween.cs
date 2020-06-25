using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AppearTween : MonoBehaviour
{
    Image image;
    Color colorBefore;

    private void Awake() {
        image = GetComponent<Image>();
        colorBefore = new Vector4(image.color.r, image.color.g, image.color.b, 0.66f);
    }

    public void Disappear()
    {
        image.DOColor(new Vector4(colorBefore.r, colorBefore.g, colorBefore.b, 0), 1f);
    }

    public void Appear()
    {
        image.DOColor(colorBefore, 1f);
    }
}

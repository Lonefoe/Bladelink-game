using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePopup : MonoBehaviour
{

    public void ShrineRangeEntered()
    {
            GetComponent<Image>().enabled = true;
    }

    public void ShrineRangeExited()
    {
            GetComponent<Image>().enabled = false;
    }

}

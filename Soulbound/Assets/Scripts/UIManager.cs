using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //-------------REFERENCES---------------//

    public Slider healthSlider;

    //--------------------------------------//

    private void Awake()
    {
        Instance = this;
    }
}

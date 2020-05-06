using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //-------------UI REFERENCES---------------//

    public Slider healthSlider;
    public TextMeshProUGUI soulPointsText;

    //--------------------------------------//

    private void Awake()
    {
        Instance = this;
    }
}

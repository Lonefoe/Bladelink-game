using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //-------------UI REFERENCES---------------//

    #region UI REFERENCES

    public Slider healthSlider;
    public TextMeshProUGUI soulPointsText;
    public TextMeshProUGUI saveTextPopup;

    #endregion

    //--------------------------------------//

    private void Awake()
    {
        Instance = this;
    }
}

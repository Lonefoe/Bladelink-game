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

    public GameObject hudCanvas;
    public GameObject pauseCanvas;
    public Slider healthSlider;
    public TextMeshProUGUI soulPointsText;
    public TextMeshProUGUI saveTextPopup;

    #endregion

    //--------------------------------------//

    private void Awake()
    {
        Instance = this;
    }

    public void HideHUD(bool hide)
    {
        if (hide) hudCanvas.SetActive(false);
        else hudCanvas.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public bool anyMenuActive { get; set; }

    //-------------UI REFERENCES---------------//

    #region UI REFERENCES

    public GameObject hudCanvas;
    public GameObject pauseCanvas;
    public GameObject backpackPanel;
    public Slider healthSlider;
    public TextMeshProUGUI soulPointsText;
    public TextMeshProUGUI saveTextPopup;

    #endregion

    //--------------------------------------//

    private void Awake()
    {
        Instance = this;
     //   InputManager.controls.Player.OpenBackpack.performed += ctx => ToggleBackpack();
    }

    public void HideHUD(bool hide)
    {
        if(hudCanvas == null) return;
        if (hide) hudCanvas.SetActive(false);
        else hudCanvas.SetActive(true);
    }

    public bool IsAnyMenuActive() { return anyMenuActive; }

    public void ToggleBackpack()
    {
        if(!anyMenuActive && !backpackPanel.activeInHierarchy)
        {
        backpackPanel.SetActive(true);
        GameManager.Instance.FreezeScreen();
        anyMenuActive = true;
        }
        else if (anyMenuActive && backpackPanel.activeInHierarchy)
        {
        backpackPanel.SetActive(false);
        GameManager.Instance.FreezeScreen();
        anyMenuActive = false;
        }
        
    }

    public void PlayButtonSound(string name)
    {
        if(name == "Enter")
        {
            AudioManager.Instance.PlayOneShot("onButton");
        }
    }
}

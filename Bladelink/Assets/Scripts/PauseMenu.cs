using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public void Awake() {
        InputManager.controls.Player.PauseGame.started += ctx => ExitGame();
    }

    public void Start()
    {
        pausePanel.SetActive(false);
    }

    private void HandlePause()
    {
        if (!pausePanel.activeInHierarchy && !UIManager.Instance.IsAnyMenuActive())
        {
            PauseGame();
        } else if (pausePanel.activeInHierarchy) 
        {
            ContinueGame();   
        } 
    }

    public void PauseGame()
    { 
        pausePanel.SetActive(true);
        GameManager.Instance.hideHUD = true;
        GameManager.Instance.FreezeScreen();
        UIManager.Instance.anyMenuActive = true;
    } 
    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.hideHUD = false;
        GameManager.Instance.FreezeScreen();
        UIManager.Instance.anyMenuActive = false;
        //enable the scripts again
    }

    void ExitGame()
    {
        Application.Quit();
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public void Awake() {
        InputManager.controls.Player.PauseGame.started += ctx => HandlePause();
    }

    public void Start()
    {
        pausePanel.SetActive(false);
    }

    private void HandlePause()
    {
        if (!pausePanel.activeInHierarchy) 
        {
            PauseGame();
        } else if (pausePanel.activeInHierarchy) 
        {
            ContinueGame();   
        } 
    }

    private void PauseGame()
    { 
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance.OnGamePaused();
        //Disable scripts that still work while timescale is set to 0
    } 
    private void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;     
        GameManager.Instance.OnGamePaused();
        //enable the scripts again
    }
 
}

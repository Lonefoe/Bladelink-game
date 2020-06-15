using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Range(0, 2f)] public float speed = 1.0f;
    [SerializeField] private float gravityMultiplier = 1f;      // Tweaker for gravity, multiplies the WORLD gravity
    [SerializeField] private bool pauseInEditor = false;
    public bool hideHUD = false;
    [SerializeField] private bool useDevTools = true;
    private bool gamePaused = false;
    private float startTimeScale;

    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Update()
    {
        if (useDevTools)
        {
            DevTools.ReloadLevel();
            DevTools.GiveSoulPoints();
            DevTools.AddEnemy();
        }

        UIManager.Instance.HideHUD(hideHUD);
    }

    private void Start()
    {
        // Tweaking the world gravity
        Physics2D.gravity *= gravityMultiplier;
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void FreezeScreen(bool usePreviousTime = false)
    {
        if(!gamePaused)
        {
        startTimeScale = Time.timeScale;
        gamePaused = true;
        Time.timeScale = 0f;
        }
        else 
        {
            if(usePreviousTime) Time.timeScale = startTimeScale;
            else Time.timeScale = 1f;
            gamePaused = false;
        }
    }

    // Called as an event, loads the next level in build
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Called as an event, loads the main menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Called as an event, exits the game
    public void ExitGame()
    {
        Application.Quit();
    }

}

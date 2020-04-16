using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float gravityMultiplier = 1f;      // Tweaker for gravity, multiplies the WORLD gravity
    [SerializeField] private bool pauseInEditor = false;
    private bool gamePaused = false;

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
        if (pauseInEditor == true)
        {
            gamePaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            gamePaused = false;
        }
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private int levelCount = 1;
    [SerializeField] private string level1Name;
    [SerializeField] private string creditsName;
    [SerializeField] private string mainMenuName;
    public bool isPaused;

    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(mainMenuName);

    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene(level1Name);
        levelCount++;
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(creditsName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (SceneManager.GetActiveScene().name == "Initial" || SceneManager.GetActiveScene().name == "CreditsScene")
            {
                return;
            }
            else
            {
                PauseGame();
            }
        }
        if(isPaused && Input.GetKeyDown(KeyCode.M))
        {
            isPaused = false;
            Time.timeScale = 1;
            LoadMainMenu();
            levelCount = 1;
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

}

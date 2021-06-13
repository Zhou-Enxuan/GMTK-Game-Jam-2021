using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private int levelCount = 0;
    [SerializeField] private string level1Name;
    [SerializeField] private string creditsName;
    [SerializeField] private string mainMenuName;

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

}

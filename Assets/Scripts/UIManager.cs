using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Components")]
    [SerializeField]private Text selectPartText;
    [SerializeField] private GameObject player;

    //Victory Placeholder
    [SerializeField] private GameObject victoryText;

    public int selection;

    private string[] partChoose;

    //ImageScrolling
    [SerializeField] private Sprite[] imageLibrary;
    [SerializeField] private Image selectedPartImage;

    //Pause Related
    [SerializeField] private GameObject pauseText;

    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        pauseText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        partChoose = new string[3];
        partChoose[0] = "Arm";
        partChoose[1] = "Head";
        partChoose[2] = "Leg";

        selection = 0;

    }

    // Update is called once per frame
    void Update()
    {
        selectingPart();
        CheckPause();
    }

    private void CheckPause()
    {
        MainMenuManager currentManager = FindObjectOfType<MainMenuManager>();
        if (!currentManager) { return; }
        
        else if (currentManager.isPaused)
        {
            pauseText.SetActive(true);
        }
        else 
        {
            pauseText.SetActive(false);
        }
    }

    private void selectingPart()
    {
        selectPartText.text = partChoose[selection];
        selectedPartImage.sprite = imageLibrary[selection];
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selection < 2)
            {
                ++selection;
            }
            else
            {
                selection = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selection > 0)
            {
                --selection;
            }
            else
            {
                selection = 2;
            }
        }
    }

    public void WinText()
    {

        victoryText.SetActive(true);

    }

    
}

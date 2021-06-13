using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Components")]
    public CharacterState state;
    [SerializeField]private Text selectPartText;
    [SerializeField] private GameObject player;

    //Victory Placeholder
    public GameObject victoryText;

    public int selection;

    private string[] partChoose;

    //ImageScrolling
    [SerializeField] private Sprite[] imageLibrary;
    [SerializeField] private Image selectedPartImage;

    //Pause Related
    [SerializeField] private GameObject pauseText;
    [SerializeField] private GameObject rButttonKey;

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
        rButttonKey.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MyRobot");
        state = player.GetComponent<CharacterState>(); 
        /*
        partChoose = new string[3];
        partChoose[0] = "Arm";
        partChoose[1] = "Head";
        partChoose[2] = "Leg";

        selection = 0;
        player.SendMessage("checkFunction", selection);
        */

    }

    // Update is called once per frame
    void Update()
    {
        selectingPart();
        CheckPause();
        CheckReset();
    }

    private void CheckReset()
    {
        state = FindObjectOfType<CharacterState>();

        if (!state.isAttached(CharacterState.bodyPart.Arm) || !state.isAttached(CharacterState.bodyPart.Leg))
        {
            rButttonKey.SetActive(true);
        }
        else if (state.isAttached(CharacterState.bodyPart.Arm) || state.isAttached(CharacterState.bodyPart.Leg))
        {
            rButttonKey.SetActive(false);
        }
        
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
        selectPartText.text = state.partSelected.ToString();
        selectedPartImage.sprite = imageLibrary[(int)state.partSelected];
        if (Input.GetKeyDown(KeyCode.E))
        {
            state.selectNext();
            /*
            if (selection < 2)
            {
                ++selection;
            }
            else
            {
                selection = 0;
            }
            */
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            state.selectPrevious();
            /*
            if (selection > 0)
            {
                --selection;
            }
            else
            {
                selection = 2;
            }
        }
        player.SendMessage("checkFunction", selection);
        */
        }
    }

    public void WinText()
    {

        victoryText.SetActive(true);

    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Componetns")]
    [SerializeField]private Text selectPartText;
    [SerializeField] private GameObject player;

    public int selection;

    private string[] partChoose;

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
    }

    private void selectingPart()
    {
        selectPartText.text = partChoose[selection];
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
}

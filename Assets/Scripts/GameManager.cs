using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] string sceneName;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void Win()
    {
        //UIManager.instance
    }
   
}

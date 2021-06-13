using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningOnstacle : MonoBehaviour
{
    [SerializeField] private string NextScene;
    // Start is called before the first frame update

    private GameObject UI;

    private GameObject Text;
    void Start()
    {
        UI = GameObject.Find("UI");
        Text = UI.transform.Find("Winning").gameObject;
        Text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Text.SetActive(true);
            StartCoroutine(startCount());
        }
        
    }

    IEnumerator startCount()
    {
        yield return new WaitForSeconds(2f);
        Text.SetActive(false);
        SceneManager.LoadScene(NextScene);
    }
}

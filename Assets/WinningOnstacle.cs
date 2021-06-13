using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningOnstacle : MonoBehaviour
{
    [SerializeField] private string NextScene;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            UIManager.instance.victoryText.SetActive(true);
            StartCoroutine(startCount());
        }
        
    }

    IEnumerator startCount()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(NextScene);
    }
}

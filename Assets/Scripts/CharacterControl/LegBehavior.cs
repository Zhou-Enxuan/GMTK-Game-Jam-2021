using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBehavior : MonoBehaviour
{

    private Rigidbody2D rb;

    private bool isHit;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isHit = false;
        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collison!");
        if (collision.transform.CompareTag("MovingPlat"))
        {
            transform.parent = collision.transform;
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }
        if (!collision.transform.CompareTag("Player"))
        { 
            Debug.Log("isHit");
            isHit = true;
        }

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("leg is " + (collision.transform.CompareTag("Player") && player.GetComponent<CharacterMovement>().isOnLeg).ToString());
        
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("it is charatcer");
            if (player.GetComponent<CharacterMovement>().isOnLeg)
            {
                player.GetComponent<ThrowPart>().pickUpLeg();
                Destroy(this.gameObject);
                player.GetComponent<ThrowPart>().breakLeg = null;
                player.GetComponent<CharacterMovement>().isOnLeg = false;
            }
        }
        
    }
}

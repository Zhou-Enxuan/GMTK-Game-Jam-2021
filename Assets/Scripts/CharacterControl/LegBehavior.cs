using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBehavior : MonoBehaviour
{

    private Rigidbody2D rb;    
    private bool isHit;
    private GameObject player;
    private float Counter;

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isHit = false;
        player = GameObject.Find("MyRobot");
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
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && player.GetComponent<CharacterMovement>().isOnLeg)
        {
            Counter += Time.deltaTime;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("leg is " + (collision.transform.CompareTag("Player") && player.GetComponent<CharacterMovement>().isOnLeg).ToString());
        
        if (collision.transform.CompareTag("Player"))
        {
            if(Counter <= 0.15f)
            {
                Counter = 0;
            }

            if (player.GetComponent<CharacterMovement>().isOnLeg && Counter > 0.15f)
            {
                player.GetComponent<ThrowPart>().pickUpLeg();
                Destroy(this.gameObject);
                player.GetComponent<ThrowPart>().breakLeg = null;
                player.GetComponent<CharacterMovement>().isOnLeg = false;
                Counter = 0;
            }
        }
    }
}

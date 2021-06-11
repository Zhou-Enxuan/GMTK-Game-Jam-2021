using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool isHit;

    private bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isHit = false;
        isPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && isHit)
        {
            collision.transform.GetComponent<ThrowPart>().pickPart();
            Destroy(this.gameObject);
        }

        if(!collision.transform.CompareTag("Player"))
        {
            Debug.Log("isHit");
            isHit = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] public float magneticForce = 0.5f;
    [SerializeField] public float magneticRadius = 5f;

    public GameObject player;
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
        Debug.Log("collison!");
        if (collision.transform.CompareTag("Player"))
        {
            if(isHit){
                collision.transform.GetComponent<ThrowPart>().pickHand();
                Destroy(this.gameObject);
            }
        } else if(collision.transform.CompareTag("Grabbable"))
        {
            Debug.Log("isHit");
            isHit = true;
        } else {
            player.GetComponent<ThrowPart>().pickHand();
            Destroy(this.gameObject);
            player.GetComponent<ThrowPart>().breakPart = null;
        }
    }
}

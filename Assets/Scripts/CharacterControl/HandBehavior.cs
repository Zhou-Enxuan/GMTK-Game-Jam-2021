using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] public float magneticForce = 0.5f;
    [SerializeField] public float magneticRadius = 5f;
    private CharacterState state;
    public GameObject player;
    private Rigidbody2D rb;

    private bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isHit = false;
        player = GameObject.Find("MyRobot");
        state = player.GetComponent<CharacterState>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

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
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.isKinematic = true;
            isHit = true;

            state.detach(CharacterState.bodyPart.Arm);
        }
        else if(collision.transform.CompareTag("MovingPlat"))
        {
            transform.parent = collision.transform;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true;
            isHit = true;
        }
        else {
            player.GetComponent<ThrowPart>().pickHand();
            Destroy(this.gameObject);
            player.GetComponent<ThrowPart>().breakHand = null;
        }
    }
}

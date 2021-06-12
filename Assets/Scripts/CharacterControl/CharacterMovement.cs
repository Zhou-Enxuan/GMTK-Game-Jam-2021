using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpfroce;

    private float moveInput;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float checkRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    public bool isConnecting;



    // Start is called before the first frame update
    void Start()
    {
        isConnecting = false;
    }

    private void Update()
    {
        if (!isConnecting)
        {
            moveInput = Input.GetAxis("Horizontal");

            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
            {
                rb.velocity = Vector2.up * jumpfroce * Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        rb.velocity = new Vector2(moveInput * speed * Time.deltaTime, rb.velocity.y);
    }
}

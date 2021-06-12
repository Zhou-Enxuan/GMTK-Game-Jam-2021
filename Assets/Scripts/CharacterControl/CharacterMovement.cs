using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement variables")]
    [SerializeField] private float movementAcceelertion;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float linearDrag;
    private float horizontalDirection;
    private bool changeDirection => (rb.velocity.x > 0 && horizontalDirection < 0) || (rb.velocity.x < 0 && horizontalDirection > 0);

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airLinearDrag;


    [Header("Ground Collision Variables")]
    [SerializeField] private float grpundRaycastLength;
    [SerializeField] private LayerMask whatIsGround;
    private bool canJump => Input.GetKey(KeyCode.W) && isGrounded;
    public bool isGrounded;

    public bool isOutControl;


    public bool isConnecting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckCollision();
        horizontalDirection = GetInput().x;
        if (canJump && !isOutControl)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(!isConnecting && !isOutControl)
        {
            MoveCharacter();
            if(isGrounded)
            {
                ApplyLinearDrag();
            }
            else
            {
                ApplyAirLinearDrag();
            }
        }

        if(isOutControl)
        {
            Running();
        }

    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceelertion);

        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y); 
        }

        if (horizontalDirection > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (horizontalDirection < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
    }

    private void ApplyLinearDrag()
    {
        if(Mathf.Abs(horizontalDirection) < 0.4f || changeDirection)
        {
            rb.drag = linearDrag;
        }
        else 
        {
            rb.drag = 0f;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, grpundRaycastLength, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * grpundRaycastLength);
    }

    private void ApplyAirLinearDrag()
    {
        rb.drag = airLinearDrag;
    }

    public void Respond(Vector2 respondPoint)
    {
        transform.position = respondPoint;
    }

    private void Running()
    {
        if(transform.localScale.x > 0)
        {
            rb.AddForce(new Vector2(1, 0f) * movementAcceelertion);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.AddForce(new Vector2(-1, 0f) * movementAcceelertion);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }


    //shoot off the leg
    //***NEED TO BE IMPLEMENT***
    //Slow down player movement
    private void limping()
    {

    }
}

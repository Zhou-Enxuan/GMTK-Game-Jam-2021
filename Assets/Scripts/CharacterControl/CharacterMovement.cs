using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //[SerializeField]
    //private float speed;
    //[SerializeField]
    //private float jumpfroce;

    //private float moveInput;
    //[SerializeField]
    //private Rigidbody2D rb;

    //[SerializeField]
    //private Transform groundCheck;

    //[SerializeField]
    //private float checkRadius;

    //[SerializeField]
    //private LayerMask whatIsGround;

    //public bool isGrounded;

    //public bool isConnecting;

    //public Transform breakpart;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    isConnecting = false;
    //}

    //private void Update()
    //{
    //    if (!isConnecting)
    //    {
    //        moveInput = Input.GetAxis("Horizontal");

    //        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
    //        {
    //            Debug.Log(isGrounded);

    //            rb.velocity = new Vector2(rb.velocity.x, 0);
    //            rb.AddForce(Vector2.up * jumpfroce);
    //        }
    //    }
    //}

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

    //    rb.velocity = new Vector2(moveInput * speed * Time.deltaTime, rb.velocity.y);
    //}

    //public void Respond(Vector2 respondPoint)
    //{
    //    transform.position = respondPoint;
    //}

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


    public bool isConnecting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckCollision();
        horizontalDirection = GetInput().x;
        if (canJump)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(!isConnecting)
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
}

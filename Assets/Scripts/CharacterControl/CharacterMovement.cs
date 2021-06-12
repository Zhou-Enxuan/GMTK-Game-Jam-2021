using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement variables")]
    
    [SerializeField]
    private GameObject respawnPoint;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float linearDrag;
    [SerializeField] private float limpingSpeedModifier = 0.5f;
    private float horizontalDirection;
    private bool changeDirection => (rb.velocity.x > 0 && horizontalDirection < 0) || (rb.velocity.x < 0 && horizontalDirection > 0);
    private Vector3 m_Velocity = Vector3.zero;
    public bool isLimping = false;

    private GameObject magneticCenter;
    private float magneticForce;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

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
            checkMagneticPull();
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
        float movement = horizontalDirection * movementSpeed * Time.fixedDeltaTime;
        if(isLimping){
            movement = movement * limpingSpeedModifier;
        }
        Vector3 targetVelocity = new Vector2(movement * 10f, rb.velocity.y);


        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        //rb.AddForce(new Vector2(horizontalDirection, 0f) * movementSpeed);

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
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

    private void Running()
    {
        if(transform.localScale.x > 0)
        {
            rb.AddForce(new Vector2(1, 0f) * movementSpeed);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.AddForce(new Vector2(-1, 0f) * movementSpeed);

            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = respawnPoint.transform.position;
    }

    public void toggleLimping()
    {
        isLimping = !isLimping;
    }

    private void checkMagneticPull(){
        if(magneticCenter != null){
            float radius = magneticCenter.GetComponent<HandBehavior>().magneticRadius;
            Vector2 distance = magneticCenter.transform.position - transform.position;
            if(distance.magnitude > radius){
                distance = (distance.normalized * (distance.magnitude - radius));
                transform.position = Vector2.Lerp(transform.position, distance, magneticForce * Time.deltaTime);
            }
        }
    }

    public void startMagneticPull(GameObject arm, float mf){
        magneticCenter = arm;
        magneticForce = mf;
    }

    public void stopMagneticPull(){
        magneticCenter = null;
    }


}

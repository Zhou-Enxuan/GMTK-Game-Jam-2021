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
    public Vector3 thisIsTheScaleNow;
    private bool changeDirection => (rb.velocity.x > 0 && horizontalDirection < 0) || (rb.velocity.x < 0 && horizontalDirection > 0);
    private Vector3 m_Velocity = Vector3.zero;
    public bool isLimping = false;

    [SerializeField] private float maxMagnetRange = 150; 
    private GameObject magneticCenter;
    private float magneticForce;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airLinearDrag;
    public float jumpFreeze = 0;


    [Header("Ground Collision Variables")]
    [SerializeField] private float grpundRaycastLength;
    [SerializeField] private LayerMask whatIsGround;
    private bool canJump => Input.GetKeyDown(KeyCode.W) && isGrounded && jumpFreeze <= 0;
    public bool isGrounded;
    public bool isOnLeg;
    [SerializeField] private LayerMask whatIsLeg;

    public bool isOutControl;


    public bool isConnecting;

    private Animator anim;

    public bool freeze = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckCollision();
        horizontalDirection = GetInput().x;
        if (!freeze && canJump && !isOutControl)
        {
            Jump();
        }

        if(jumpFreeze > 0)
        {
            jumpFreeze -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isConnecting && !isOutControl && !freeze)
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
        checkMagneticPull();
        AniamtionsVariableCheck();

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

        anim.SetFloat("Speed", Mathf.Abs(movement));

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        //rb.AddForce(new Vector2(horizontalDirection, 0f) * movementSpeed);

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            transform.localScale = new Vector3(thisIsTheScaleNow.x, thisIsTheScaleNow.y, thisIsTheScaleNow.z);
        }
        else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            transform.localScale = new Vector3(-thisIsTheScaleNow.x, thisIsTheScaleNow.y, thisIsTheScaleNow.z);
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
        Debug.Log("We jumped");
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
        jumpFreeze = 2;
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, grpundRaycastLength, whatIsGround);
        if(Physics2D.Raycast(transform.position, Vector2.down, grpundRaycastLength, whatIsLeg))
        {
            isOnLeg = true;
        }
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
        float direction = 1;
        if(transform.localScale.x < 0)
        {
            direction = -1;
        }

        float movement = direction * movementSpeed * Time.fixedDeltaTime;
        if(isLimping){
            movement = movement * limpingSpeedModifier;
        }
        Vector3 targetVelocity = new Vector2(movement * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
            if(distance.magnitude > maxMagnetRange){
                return;
            }
            if(distance.magnitude > radius){
                distance = (distance.normalized * (distance.magnitude - radius));
                transform.position = Vector2.Lerp(transform.position, distance, magneticForce * Time.fixedDeltaTime);
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

    private void AniamtionsVariableCheck()
    {
        anim.SetBool("Limp", isLimping);
        anim.SetBool("IsGround", isGrounded);
        anim.SetFloat("AirSpeed", rb.velocity.y);
    }

    private void FallToGroundEnter()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector2.zero, ref m_Velocity, m_MovementSmoothing);
        freeze = true;
    }

    private void FallToGroundExit()
    {
        freeze = false;
        anim.ResetTrigger("Jump");
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPart : MonoBehaviour
{
    [SerializeField]
    private GameObject partToThrow;

    [SerializeField]
    private Transform myBodyPart;

    [SerializeField]
    private float lunchForce;

    [SerializeField]
    private Transform shotPoint;

    [SerializeField] private GameObject lefthand;

    [SerializeField] private Transform HandShootingPoint;

    [SerializeField] private float angel;

    // Start is called before the first frame update

    [SerializeField]
    private float retractAcce;

    public GameObject breakHand;

    public GameObject breakLeg;

    private bool canRetract;

    public event Action Callback;

    public float HeadBack = 5;

    private Animator anim;


    void Start()
    {
        myBodyPart = transform.Find("LeftHand");
        Debug.Log(myBodyPart);
        shotPoint = myBodyPart;
        canRetract = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkFunction();
            Vector2 partPosition = lefthand.transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - partPosition;
            if (transform.localScale.x > 0 && Vector2.Angle(Vector2.right, direction) < angel)
            {
                lefthand.transform.right = direction;
            }
            else if(transform.localScale.x < 0 && Vector2.Angle(-Vector2.right, direction) < angel)
            {
                lefthand.transform.right = -direction;
            }




        //else
        //{
        //    if (transform.localScale.x > 0)
        //    {
        //        transform.Find("LeftHand").rotation = Quaternion.Euler(0f, 0f, -90f);
        //    }
        //    else
        //    {
        //        transform.Find("LeftHand").rotation = Quaternion.Euler(0f, 0f, 90f);

        //    }

        //}
        if(myBodyPart.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {

            Callback?.Invoke();
        }

        if (breakHand != null)
            Retract();

    }

    //shooting off the arm
    private void ShootArm()
    {
       
        anim.SetTrigger("ShotHand");
        GetComponent<CharacterMovement>().freeze = true;
        //GameObject.Find("Main Camera").GetComponent<mainCamera>().followPart(breakHand);
    }

    private void ShootArmActivate()
    {
        breakHand = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        breakHand.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        breakHand.GetComponent<HandBehavior>().player = gameObject;
        GetComponent<CharacterMovement>().startMagneticPull(breakHand, breakHand.GetComponent<HandBehavior>().magneticForce);
        breakHand.GetComponent<Rigidbody2D>().AddForce(breakHand.transform.right * lunchForce * (transform.localScale.x * 2));
        lefthand.SetActive(false);
        GetComponent<CharacterMovement>().freeze = false;

    }

    //shoot off the head
    private void ShootHead()
    {
        myBodyPart.gameObject.SetActive(false);
        GetComponent<CharacterMovement>().isOutControl = true; 
        StartCoroutine(HeadBackCount());

    }

    //shoot off the leg
    //***NEED TO BE IMPLEMENT***
    //Shoot the leg at horizontallly and when the leg hit a collider it freeze and become platfrom to jump on
    private void ShootLeg()
    {
        breakLeg = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        breakLeg.GetComponent<Rigidbody2D>().AddForce(breakLeg.transform.right * lunchForce * (transform.localScale.x * 2));
        breakLeg.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        GetComponent<CharacterMovement>().toggleLimping();
        myBodyPart.gameObject.SetActive(false);
        GetComponent<CharacterMovement>().isOnLeg = false;
    }

    //pick up the hand after the player collide with the hand
    public void pickHand()
    {
        Debug.Log("hand is pick up");
        lefthand.SetActive(true);
        GetComponent<CharacterMovement>().isConnecting = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 10;
        GetComponent<CharacterMovement>().stopMagneticPull();
        anim.ResetTrigger("ShotHand");
        //GameObject.Find("Main Camera").GetComponent<mainCamera>().restoreFollow();
    }

    //check which selection we have and set up the correct behavior
    private void checkFunction()
    {
        switch (UIManager.instance.selection)
        {
            case 0:
                partToThrow = Resources.Load<GameObject>("Prefab/Robot/Part/LeftHand");
                myBodyPart = transform.Find("LeftHand");
                shotPoint = HandShootingPoint;
                Callback = ShootArm;
                break;
            case 1:
                partToThrow = Resources.Load<GameObject>("Prefab/Robot/Part/Head");
                myBodyPart = transform.Find("Head");
                Callback = ShootHead;
                break;
            case 2:
                partToThrow = Resources.Load<GameObject>("Prefab/Robot/Part/LeftLeg");
                myBodyPart = transform.Find("LeftLeg");
                shotPoint = transform.Find("LegShotPoint");
                Callback = ShootLeg;
                break;
            default:
                break;
        }

    }

    //retract player to the hand
    private void Retract()
    {
        Vector2 partDirection = -(transform.position - breakHand.transform.position).normalized;
        //Debug.Log(partDirection);
        Debug.DrawRay(transform.position, partDirection, Color.red);
        if (Input.GetMouseButton(1) && canRetract)
        {
            transform.position = Vector2.Lerp(transform.position, breakHand.transform.position, retractAcce * Time.deltaTime);
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<CharacterMovement>().isConnecting = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            GetComponent<CharacterMovement>().isConnecting = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 10;
            if (GetComponent<CharacterMovement>().isGrounded)
            {
                canRetract = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            pickHand();
            Destroy(breakHand.gameObject);
            breakHand = null;
        }
    }

    IEnumerator HeadBackCount()
    {
        yield return new WaitForSeconds(HeadBack);
        transform.Find("Head").gameObject.SetActive(true);
        GetComponent<CharacterMovement>().isOutControl = false;
    }

    //shoot off the leg
    //***NEED TO BE IMPLEMENT***
    //After you stand up on the leg platform, the next jump brought your leg back
    public void pickUpLeg()
    {
        Debug.Log("leg pick up");
        transform.Find("LeftLeg").gameObject.SetActive(true);
        if (GetComponent<CharacterMovement>().isLimping){
            GetComponent<CharacterMovement>().toggleLimping();
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Player") && GetComponent<CharacterMovement>().isConnecting && !GetComponent<CharacterMovement>().isGrounded)
        {
            GetComponent<CharacterMovement>().isConnecting = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 10;
            canRetract = false;
        }
    }
}

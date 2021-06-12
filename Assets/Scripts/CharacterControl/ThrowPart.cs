using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPart : MonoBehaviour
{
    [SerializeField]
    private GameObject partToThrow;

    private Transform myBodyPart;

    [SerializeField]
    private float lunchForce;

    [SerializeField]
    private Transform shotPoint;
    // Start is called before the first frame update

    [SerializeField]
    private float retractAcce;

    public GameObject breakPart;

    private bool canRetract;

    public event Action Callback;

    public float HeadBack = 5;


    void Start()
    {
        myBodyPart = transform.Find("RightHand");
        shotPoint = myBodyPart.transform;
        canRetract = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkFunction();
        if (myBodyPart.gameObject.activeSelf)
        {
            if (myBodyPart.name == "RightHand")
            {
                Vector2 partPosition = myBodyPart.transform.position;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mousePosition - partPosition;
                if (transform.localScale.x > 0 && Vector2.Angle(Vector2.right, direction) < 45)
                {
                    myBodyPart.right = direction;
                }
                else if(transform.localScale.x < 0 && Vector2.Angle(-Vector2.right, direction) < 45)
                {
                    myBodyPart.right = -direction;
                }
                

            }

            else
            {
                if (transform.localScale.x > 0)
                {
                    transform.Find("RightHand").rotation = Quaternion.Euler(0f, 0f, -90f);
                }
                else
                {
                    transform.Find("RightHand").rotation = Quaternion.Euler(0f, 0f, 90f);

                }

            }
            if(Input.GetMouseButtonDown(0))
            {
               Callback?.Invoke();
            }
        }
        if(breakPart != null)
            Retract();

    }

    //shooting off the arm
    private void ShootArm()
    {
        breakPart = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        breakPart.GetComponent<HandBehavior>().player = gameObject;
        GetComponent<CharacterMovement>().startMagneticPull(breakPart, breakPart.GetComponent<HandBehavior>().magneticForce);
        breakPart.GetComponent<Rigidbody2D>().AddForce(breakPart.transform.right * lunchForce * (transform.localScale.x * 2));
        myBodyPart.gameObject.SetActive(false);
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
        //breakPart = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        //breakPart.GetComponent<Rigidbody2D>().AddForce(breakPart.transform.right * lunchForce * (transform.localScale.x * 2));
        //GetComponent<CharacterMovement>().toggleLimping();
        //myBodyPart.gameObject.SetActive(false);
    }

    //pick up the hand after the player collide with the hand
    public void pickHand()
    {
        transform.Find("RightHand").gameObject.SetActive(true);
        GetComponent<CharacterMovement>().isConnecting = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 10;
        GetComponent<CharacterMovement>().stopMagneticPull();
    }

    //check which selection we have and set up the correct behavior
    private void checkFunction()
    {
        switch (UIManager.instance.selection)
        {
            case 0:
                partToThrow = Resources.Load<GameObject>("Prefab/Character/Part/RightHand");
                myBodyPart = transform.Find("RightHand");
                Callback = ShootArm;
                break;
            case 1:
                partToThrow = Resources.Load<GameObject>("Prefab/Character/Part/Head");
                myBodyPart = transform.Find("Head");
                Callback = ShootHead;
                break;
            case 2:
                partToThrow = Resources.Load<GameObject>("Prefab/Character/Part/RightLeg");
                myBodyPart = transform.Find("RightLeg");
                Callback = ShootLeg;
                break;
            default:
                break;
        }

    }

    //retract player to the hand
    private void Retract()
    {
        Vector2 partDirection = -(transform.position - breakPart.transform.position).normalized;
        //Debug.Log(partDirection);
        Debug.DrawRay(transform.position, partDirection, Color.red);
        if (Input.GetMouseButton(1) && canRetract)
        {
            transform.position = Vector2.Lerp(transform.position, breakPart.transform.position, retractAcce * Time.deltaTime);
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
            Destroy(breakPart.gameObject);
            breakPart = null;
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
    private void pickUpLeg()
    {
        if(!GetComponent<CharacterMovement>().isLimping){
            GetComponent<CharacterMovement>().toggleLimping();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GetComponent<CharacterMovement>().isConnecting = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().gravityScale = 10;
            canRetract = false;
        }
    }
}

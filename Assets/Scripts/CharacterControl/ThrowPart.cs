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

    private GameObject breakPart;

    private bool canRetract;


    void Start()
    {
        myBodyPart = transform.Find("RightHand");
        shotPoint = myBodyPart.transform;
        canRetract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (myBodyPart.gameObject.activeSelf)
        {
            Vector2 partPosition = partToThrow.transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - partPosition;
            myBodyPart.right = direction;

            if(Input.GetMouseButtonDown(0))
            {
                    Shoot();
            }
        }
        else
        {
            
            Vector2 partDirection = -(transform.position - breakPart.transform.position).normalized;
            Debug.Log(partDirection);
            Debug.DrawRay(transform.position, partDirection, Color.red);
            if (Input.GetKey(KeyCode.Q) && canRetract)
            {
                transform.position = Vector2.Lerp(transform.position, breakPart.transform.position, retractAcce * Time.deltaTime);
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<CharacterMovement>().isConnecting = true;
                Debug.Log("q is down");
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                GetComponent<CharacterMovement>().isConnecting = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale = 10;
                if (GetComponent<CharacterMovement>().isGrounded)
                { 
                    canRetract = true;
                }   

            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                pickPart();
                Destroy(breakPart.gameObject);
            }
        }

    }

    private void Shoot()
    {
        breakPart = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        breakPart.GetComponent<Rigidbody2D>().AddForce(breakPart.transform.right * lunchForce);
        myBodyPart.gameObject.SetActive(false);
    }

    public void pickPart()
    {
        myBodyPart.gameObject.SetActive(true);
        GetComponent<CharacterMovement>().isConnecting = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 10;
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

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

    private GameObject breakPart;

    void Start()
    {
        myBodyPart = transform.Find(partToThrow.name);
        shotPoint = myBodyPart.transform;
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
            if (Input.GetKey(KeyCode.Q))
            {
                GetComponent<CharacterMovement>().isConnecting = true;
                //GetComponent<Rigidbody2D>().velocity = (partDirection * 500 * Time.deltaTime);
                Vector2.MoveTowards(transform.position, breakPart.transform.position, 500);
                GetComponent<Rigidbody2D>().gravityScale = 0;
                Debug.Log("is click");
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                GetComponent<CharacterMovement>().isConnecting = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }

    }

    private void Shoot()
    {
        breakPart = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        GetComponent<CharacterMovement>().breakpart = breakPart.transform;
        breakPart.GetComponent<Rigidbody2D>().AddForce(breakPart.transform.right * lunchForce);
        myBodyPart.gameObject.SetActive(false);
    }
}

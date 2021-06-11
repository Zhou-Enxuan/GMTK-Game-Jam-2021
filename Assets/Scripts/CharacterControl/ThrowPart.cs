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
            if(Input.GetKeyDown(KeyCode.Q))
            {
                GetComponent<CharacterMovement>().isConnecting = true;
                Vector3.MoveTowards(transform.position, breakPart.transform.position, 0.1f);
            }

            if(Input.GetKeyUp(KeyCode.Q))
            {
                GetComponent<CharacterMovement>().isConnecting = false;
            }
        }
    }

    private void Shoot()
    {
        breakPart = Instantiate(partToThrow, shotPoint.position, shotPoint.rotation);
        breakPart.GetComponent<Rigidbody2D>().velocity = breakPart.transform.right * lunchForce;
        myBodyPart.gameObject.SetActive(false);
    }
}

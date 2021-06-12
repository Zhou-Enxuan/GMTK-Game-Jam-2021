using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject endPointA;
    [SerializeField] private GameObject endPointB;
    [SerializeField] public float speed;
   
    private Vector3 currentDestination;

    // Start is called before the first frame update
    void Start()
    {
        currentDestination = endPointA.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DestinationCheck();
        GoToDestination();
    }

    private void DestinationCheck()
    {
        if(gameObject.transform.position == currentDestination)
        {
            if (currentDestination == endPointA.transform.position)
            {
                currentDestination = endPointB.transform.position;
            }

            else if (currentDestination == endPointB.transform.position)
            {
                currentDestination = endPointA.transform.position;
            }
        }
        else if (gameObject.transform.position != currentDestination)
        {
            Debug.Log("return");
        }
    }

    private void GoToDestination()
    {
        var actualSpeed = speed * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, currentDestination, actualSpeed);
    }
}

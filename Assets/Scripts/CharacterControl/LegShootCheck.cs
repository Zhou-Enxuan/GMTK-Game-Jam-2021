using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegShootCheck : MonoBehaviour
{
    private GameObject player;
    private int legCollisions = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MyRobot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player"))
        {
            player.GetComponent<ThrowPart>().canShootLeg = false;
            legCollisions++;
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        legCollisions--;
        if (legCollisions <= 0)
            player.GetComponent<ThrowPart>().canShootLeg = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject RespawnPoint;
    [SerializeField] private GameObject Character; 
    
    void OnCollisionEnter2D(Collision2D col){
        Debug.Log("collision");
        if(col.transform.CompareTag("Player")){
            Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Character.transform.position = RespawnPoint.transform.position;
        }
    }
}

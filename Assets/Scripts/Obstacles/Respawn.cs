using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col){
        if(col.transform.CompareTag("Player")){
            col.gameObject.GetComponent<CharacterMovement>().Respawn();
        }
    }
}

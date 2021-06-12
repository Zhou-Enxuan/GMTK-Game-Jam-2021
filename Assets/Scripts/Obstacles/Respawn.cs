using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Respawn : MonoBehaviour
/*
This class implements the respawn capabilities to an object.
extend this class to add specifics to the way the respawn happens
ovverriding OnCollisionEnter2D.
*/
{
    //method to override, remember to call "base.OnCollisionEnter2D(Collision2D col)" in the override method
    public virtual void OnCollisionEnter2D(Collision2D col){
        if(col.transform.CompareTag("Player")){
            col.gameObject.GetComponent<CharacterMovement>().Respawn();
        }
    }
}

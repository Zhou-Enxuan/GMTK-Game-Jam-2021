using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    private CharacterState state;
    private int climbables = 0; 
    private int hangables = 0;
    // Start is called before the first frame update
    void Start()
    {
        state = gameObject.GetComponentInParent<CharacterState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Grabbable")){
            state.climbing = true;
            climbables++;
            Debug.Log("Climbing: state.climbing="+state.climbing);
        }
        if(collision.CompareTag("Hangable")){
            hangables++;
            state.hanging = true;
            Debug.Log("Climbing: state.hanging="+state.hanging);
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag("Grabbable")){
            climbables--;
            if(climbables <= 0){
                state.climbing = false;
                Debug.Log("Climbing: state.climbing="+state.climbing);
            }
        }
        if(collision.CompareTag("Hangable")){
            hangables--;
            if(hangables <= 0){
                state.hanging = false;
                Debug.Log("Climbing: state.hanging="+state.hanging);
            }
        }
    }
}

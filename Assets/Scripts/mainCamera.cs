using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    private Vector3 targetPos;
    [Range(5, 20)] [SerializeField] private float cameraDistance = 10;
    [Range(-3, 3)] [SerializeField] private float cameraHeigth = 2;
    [Range(-5, 5)] [SerializeField] private float cameraWidth = 0;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float margin = 0;

    [SerializeField] private GameObject player;
    [Range(1, 10)] [SerializeField] private float interpolationRatio = 2;
    // Start is called before the first frame update
    
    void Start()
    {
        targetPos = player.transform.position - offset;
        transform.position = targetPos;

        if(player==null){
            player = GameObject.Find("Character");
        }
    }

    void FixedUpdate(){
        follow();        
    }

    private void follow(){
        offset = new Vector3(-cameraWidth, -cameraHeigth, cameraDistance);
        targetPos = player.transform.position - offset;

        if((transform.position - targetPos).magnitude >= margin){
            transform.position = Vector3.Lerp(transform.position, targetPos, interpolationRatio*Time.fixedDeltaTime);
        }
    }
}

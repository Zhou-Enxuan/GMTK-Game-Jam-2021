using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [Header("Components")]
    private GameObject player;
    public enum bodyPart{
        Arm = 0,
        Head = 1,
        Leg = 2
    };

    private bool[] partAttached;
    public bodyPart partSelected;

    public bool climbing = false;
    public bool hanging = false;
    public bool grounded = true;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        partAttached = new bool[3] {true,true,true};
        partSelected = 0;

        player.SendMessage("checkFunction", (int)partSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isAttached(bodyPart part){
        return partAttached[(int)part];
    }

    public void attach(bodyPart part){
        partAttached[(int)part] = true;
    }

    public void detach(bodyPart part){
        partAttached[(int)part] = false;
    }

    public bodyPart selectNext(){
        int part = ((int)partSelected+1 ) % 3; 
        partSelected = (bodyPart)(part);
        Debug.Log(part);
        player.SendMessage("checkFunction", (int)partSelected);
        return partSelected;
    }
    public bodyPart selectPrevious(){
        int part = (int)partSelected;
        if(part == 0)
            part = 2;
        else
            part--; 

        partSelected = (bodyPart)(part);
        Debug.Log(part);
        player.SendMessage("checkFunction", (int)partSelected);
        return partSelected;
    }
}

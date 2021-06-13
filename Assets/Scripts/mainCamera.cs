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

    [SerializeField] private float followTime = 1;
    [SerializeField] private GameObject player;
    private GameObject alternate;
    [Range(1, 10)] [SerializeField] private float interpolationRatio = 2;

    public float x_min;
    public float x_max;
    public float y_min;
    public float y_max;
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

    void LateUpdate()
    {
        float x = Mathf.Clamp(player.transform.position.x, x_min, x_max);
        float y = Mathf.Clamp(player.transform.position.y, y_min, y_max);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
    }

    private void follow(){
        offset = new Vector3(-cameraWidth, -cameraHeigth, cameraDistance);
        targetPos = player.transform.position - offset;

        if((transform.position - targetPos).magnitude >= margin){
            transform.position = Vector3.Lerp(transform.position, targetPos, interpolationRatio*Time.fixedDeltaTime);
        }
    }

    public IEnumerator followPart(GameObject brokenPart){
        float newMargin = 0;
        if(alternate == null)
            alternate = player;
        player = brokenPart;
        yield return new WaitForSeconds(followTime);
        restoreFollow();
    }

    public void restoreFollow(){
        if(alternate != null){
            player = alternate;
            alternate = null;
        }
    }
}

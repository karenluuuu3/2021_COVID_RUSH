using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseControl : MonoBehaviour
{
    public Transform target;
    public float x;
    public float y;
    public float xSpeed ; //mouse dpi
    public float ySpeed ; //mouse spi
    public float distance; //camera distance to target
    public float disSpeed; //mouse rolling dpi
    public float minDis;
    public float maxDis;

    private Quaternion rotationEuler;
    private Vector3 cameraPos;

    // Start is called before the first frame update
    void InitVaraibles (){
        target = GameObject.Find("Player").GetComponent<Transform>();
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    void Start()
    {
        InitVaraibles();
    }

    void rotationWithMouse (){
        x+= (Input.GetAxis("Mouse X"))*xSpeed*Time.deltaTime;
        y-=(Input.GetAxis("Mouse Y"))*ySpeed*Time.deltaTime;
        distance = (target.position - transform.position).magnitude;
        if (x>360)  x-=360;
        else if (x<0)   x+=360;

        distance -= Input.GetAxis("Mouse ScrollWheel")*disSpeed*Time.deltaTime;
        distance = Mathf.Clamp(distance,minDis,maxDis);
        rotationEuler = Quaternion.Euler(y,x,0);
        cameraPos = rotationEuler*new Vector3(0,0,-distance) + target.position;
        
        Debug.Log(distance);

        

        transform.rotation = rotationEuler;
        transform.position = cameraPos;
    }

    // Update is called once per frame
    void Update()
    {
        //rotationWithMouse ();
        
    }
    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        rotationWithMouse ();
    }
}

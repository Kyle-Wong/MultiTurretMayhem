using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

    // Use this for initialization
    private Transform myTransform;
    public bool playOnStartUp = true;
    private bool isRotating;
    public float rotSpeed; //I think this is radians per second?
    private Vector3 rotVector;
	void Start () {
        myTransform = transform;

        rotVector = new Vector3(0, 0, rotSpeed);
  
        isRotating = playOnStartUp;
	}
	
	// Update is called once per frame
	void Update () {
        if (isRotating)
        {
            transform.Rotate(rotVector * Time.deltaTime);
        }
    }
    public void startRotation()
    {
        isRotating = true;
    }
    public void stopRotation()
    {
        isRotating = false;
    }
    public void setSpeed(float speed)
    {
        rotSpeed = speed;
        rotVector = new Vector3(0, 0, rotSpeed);
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

    // Use this for initialization
    private Transform myTransform;
    public bool playOnStartUp = true;
    private bool isRotating;
    public float rotSpeed; //Probably degrees per second
    public float deltaRotSpeed;
    public bool randomDirection = false;

    private Vector3 rotVector;
	void Start () {
        myTransform = transform;

        rotVector = new Vector3(0, 0, rotSpeed);
        rotSpeed += Random.Range(-deltaRotSpeed / 2, deltaRotSpeed / 2);
        if (randomDirection && Random.Range(0, 1) < 0.5f)
            rotSpeed *= -1;
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

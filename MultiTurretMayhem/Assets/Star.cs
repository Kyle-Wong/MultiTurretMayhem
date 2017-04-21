using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    // Use this for initialization
    private float bounds;
    private bool isMoving;
    public float acceleration;
    public float maxSpeed;
    public float maxLength;
    public float minLength;
    public float growthSpeed;
    private Vector3 velocityVector;
    private Vector3 accelerationVector;
	void Start () {
        accelerationVector = new Vector3(0, -acceleration, 0);
        velocityVector = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            if(velocityVector.y > -maxSpeed)
            {
                velocityVector += accelerationVector * Time.deltaTime;
                
            }
            if(transform.localScale.y < maxLength)
            {
                transform.localScale += new Vector3(0, growthSpeed, 0) * Time.deltaTime;
            }
            transform.position += velocityVector * Time.deltaTime;
            
        } else
        {
            if(velocityVector.y < 0)
            {
                velocityVector -= accelerationVector * Time.deltaTime;
            }
            else
            {
                velocityVector = new Vector3(0, 0, 0);
            }
            if (transform.localScale.y > minLength)
            {
                transform.localScale -= new Vector3(0, growthSpeed, 0) * Time.deltaTime;
            }
            transform.position += velocityVector * Time.deltaTime;
        }
        if(transform.position.y < -bounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2 * bounds, 0);
        }
	}
    public void setBounds(float y)
    {
        bounds = y;
    }
    public void startMoving()
    {
        isMoving = true;
        transform.rotation = Quaternion.identity;
        
    }
    public void stopMoving()
    {
        isMoving = false;
    }
    public float getSpeed()
    {
        return Mathf.Abs(velocityVector.y);
    }
}

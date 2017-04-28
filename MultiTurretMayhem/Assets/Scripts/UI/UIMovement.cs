using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovement : MonoBehaviour {

    // Use this for initialization
    public enum MovementType
    {
        rate,
        duration
    }
    public bool playOnStartUp;
    public Transform endLocation;
    private Vector3 initialLocation;
    public MovementType movementType;
    public float rate;
    public float duration;
    private float timer;
    public float tolerance;
    private bool isMoving;
	void Start () {
        initialLocation = transform.position;
        if (playOnStartUp)
            startMovement();
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            switch (movementType)
            {
                case (MovementType.rate):
                    transform.position += (endLocation.position - transform.position).normalized * rate * Time.deltaTime;
                    break;
                case (MovementType.duration):
                    if (timer <= duration)
                    {
                        timer += Time.deltaTime;
                        transform.position = Vector3.Lerp(initialLocation, endLocation.position, timer / duration);
                    }
                    break;
            }
            if (Vector3.Distance(transform.position, endLocation.position) <= tolerance)
            {
                transform.position = endLocation.position;
                stopMovement();
            }
        }
	}
    public void startMovement()
    {
        isMoving = true;
    }
    public void stopMovement()
    {
        isMoving = false;
    }
    public void reset()
    {
        stopMovement();
        transform.position = initialLocation;
    }
}

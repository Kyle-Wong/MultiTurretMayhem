using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Movement : MonoBehaviour {

    public float velocity;

    //Base size for your object
    public float size;

    // Change this from 0 if you want sizes to be different per spawn.
    public float sizeDeviation = 0;


    // Use this for initialization
    void Start () {

        //Velocity will be set to move towards the center
        var dir = new Vector3(0,0,0) - transform.position;

        this.GetComponent<Rigidbody2D>().velocity =  dir.normalized * velocity;
        float newSize = size + (Random.Range(0f, 1f) * sizeDeviation);
        this.transform.localScale = new Vector3(newSize, newSize, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

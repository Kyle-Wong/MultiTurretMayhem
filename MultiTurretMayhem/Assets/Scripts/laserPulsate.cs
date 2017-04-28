using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPulsate : MonoBehaviour {

    // Use this for initialization
    public float speed;
    public float magnitude;
    private float elapsedTime;
	void Start () {
        elapsedTime = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        elapsedTime += speed * Time.deltaTime;
        transform.localScale += new Vector3(Mathf.Cos(elapsedTime) * magnitude,0,0) * Time.deltaTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour {

    // Use this for initialization
    private Transform playerTransform;
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.LookRotation(Vector3.back, playerTransform.position-transform.position);
    }
}

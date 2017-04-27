using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Movement : MonoBehaviour {

    public float dirVelocity;
    public float tanVelocity;
    private Transform playerTransform;
    //Base size for your object
    public bool randomSize = false;
    public float size;

    // Change this from 0 if you want sizes to be different per spawn.
    public float sizeDeviation = 0;

    private Vector3 dir;
    private Vector3 tangent;
    public bool stopped = false;
    // Use this for initialization
    void Start () {

        //Velocity will be set to move towards the center
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        dir = new Vector3(0,0,0) - transform.position;

        GetComponent<Rigidbody2D>().velocity = dir.normalized * dirVelocity + orthoDir() * tanVelocity;
        if (randomSize)
        {
            float newSize = size + (Random.Range(0f, 1f) * sizeDeviation);
            transform.localScale = new Vector3(newSize, newSize, 1);
        }

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!stopped)
        {
            dir = getPlayerDir();
            tangent = orthoDir();
            GetComponent<Rigidbody2D>().velocity = tangent * tanVelocity + dir.normalized * dirVelocity;
        }
        
    }
    public Vector3 getPlayerDir()
    {
        return (playerTransform.position-transform.position).normalized;
    }

    public Vector3 orthoDir()
    {
        Vector3 ortho = new Vector3();
        Vector3 temp_dir = getPlayerDir(); 
        ortho.x = dir.y;
        ortho.y = -dir.x;
        return ortho;
    }
    public void stopMovement()
    {
        stopped = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}

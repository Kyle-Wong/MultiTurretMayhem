using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour {

    // Use this for initialization
    
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            explode(50);
        }
	}
    public void explode(int particleNum)
    {
        for(int i = 0; i < particleNum; i++)
        {
            int particleType = (int)Random.Range(1, 3.99999999f);
            float rngAngle = Random.Range(0, 360);
            float rngSpeed = 1f + Random.Range(0, 3f);
            float rngRot = Random.Range(-150, 150);
            GameObject particle = (GameObject)Instantiate(Resources.Load("Particle"+particleType));
            particle.GetComponent<ConstantMovement>().setVelocity(rngAngle, rngSpeed);
            particle.GetComponent<ConstantRotation>().setSpeed(rngRot);
            particle.transform.position = new Vector3(0, 0, 0);
        }    
    }
}

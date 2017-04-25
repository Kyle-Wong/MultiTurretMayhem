using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFountain : MonoBehaviour {

    // Use this for initialization
    public bool playOnStartUp = true;
    public Vector3 direction;
    public int particlesPerSpray;
    public float offSet;
    public float degreesDeviation;
    public float minVelocity;
    public float maxVelocity;
    public float fireDelay;
    
    private float fireTimer;
	void Start () {
        fireTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(fireTimer < fireDelay)
        {
            fireTimer += Time.deltaTime;
        } else
        {
            fireTimer = 0;
            expelParticles();
        }
	}
    public void expelParticles()
    {
        for (int i = 0; i < particlesPerSpray; i++)
        {
            int particleType = (int)Random.Range(1, 3.99999999f);
            //
            Vector3 rngAngle = Quaternion.Euler(Random.Range(-degreesDeviation, degreesDeviation), 0, 0)*direction;
            print(direction + " " + rngAngle);
            float rngSpeed = Random.Range(minVelocity, maxVelocity);
            float rngRot = Random.Range(-150, 150);
            GameObject particle = (GameObject)Instantiate(Resources.Load("Particle" + particleType));
            particle.GetComponent<ConstantMovement>().setVelocity(rngAngle.normalized*rngSpeed);
            particle.GetComponent<ConstantRotation>().setSpeed(rngRot);
            particle.transform.position = transform.position + direction*offSet;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy {

    public GameObject missile;
    public float orbitRadius;
    public float fireDelay;
    private float fireTimer;
    public float missileOffSet;

    private Transform playerTransform;
    private Simple_Movement movement;
    private bool shootPhase;
	// Use this for initialization
	void Start () {
        baseStart();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GetComponent<Simple_Movement>();
        fireTimer = 0;
        shootPhase = false;
	}
	
	// Update is called once per frame
	void Update () {
        baseUpdate();
        transform.rotation = Quaternion.LookRotation(Vector3.back, transform.position - playerTransform.position);
        
        if (shootPhase && !isDead)
        {
            movement.dirVelocity = 0;
            if(fireTimer < fireDelay)
            {
                fireTimer += Time.deltaTime;
            } else
            {
                fireTimer = 0;
                spawnMissile();
            }
        } else
        {
            if (Vector3.Distance(playerTransform.position, transform.position) <= orbitRadius)
            {
                shootPhase = true;
            }
        }
	}
    private void spawnMissile()
    {
        Instantiate(missile, transform.position+(playerTransform.position-transform.position).normalized*missileOffSet, 
            Quaternion.LookRotation(Vector3.back, playerTransform.position-transform.position));
    }
}

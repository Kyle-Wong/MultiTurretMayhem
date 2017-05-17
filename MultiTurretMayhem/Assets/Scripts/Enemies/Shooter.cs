using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy {

    public GameObject missile;
    public float orbitRadius;
    public float fireDelay;
    private float fireTimer;
    public float missileOffSet;
    public AudioClip shootSound;

    private Transform playerTransform;
    private Transform spriteTransform;
    private Simple_Movement movement;
    private Rigidbody2D rb;
    private bool shootPhase;
    private int direction;
	// Use this for initialization
	void Start () {
        baseStart();
        spriteTransform = transform.FindChild("ShipSprite");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GetComponent<Simple_Movement>();
        direction = Random.Range(0.0f,1.0f) < 0.5 ? 1 : -1;
        movement.tanVelocity = direction * movement.tanVelocity;
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 0;
        shootPhase = false;
	}
	
	// Update is called once per frame
	void Update () {
        baseUpdate();
        
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
        if (!isDead)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, rb.velocity);
        }

    }
    private void spawnMissile()
    {
        Instantiate(missile, transform.position+(playerTransform.position-transform.position).normalized*missileOffSet, 
            Quaternion.LookRotation(Vector3.back, playerTransform.position-transform.position));
        HelperFunctions.playSound(ref _audioSource, shootSound);
    }
    
}

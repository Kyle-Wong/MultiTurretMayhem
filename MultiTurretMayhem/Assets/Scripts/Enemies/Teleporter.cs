using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Enemy {
    public float minDistance;
    private Transform target;
    public AudioClip teleportSound;

    //void Awake()
    //{
    //    target = GameObject.Find("Spawner").transform;
    //}

    // Use this for initialization
    void Start ()
    {
        baseStart();
        baseHP = 200;
        curHP = 200;
        target = GameObject.Find("Spawner").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        baseUpdate();
	}

    public override void takeDamage(int damage, Color deathColor)
    {
        base.takeDamage(damage, deathColor);
        if (curHP > 0)
            teleport();
    }

    public void teleport()
    {
        HelperFunctions.playSound(ref _audioSource, teleportSound);

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < minDistance)
            transform.position = target.position + HelperFunctions.lineVector(HelperFunctions.directionBetween(transform.position, target.position), minDistance);
        else
            transform.position = target.position + HelperFunctions.lineVector(HelperFunctions.directionBetween(transform.position, target.position), distance);

        transform.Rotate(0, 0, 180);
    }
}

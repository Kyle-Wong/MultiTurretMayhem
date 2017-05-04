using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : Enemy {
    public float tanSpeed = 3;

	// Use this for initialization
	void Start ()
    {
        baseStart();
        baseHP = 1;
        curHP = 1;
        GetComponent<Simple_Movement>().tanVelocity = Random.Range(-tanSpeed, tanSpeed);
	}
	
	// Update is called once per frame
	void Update ()
    {
        baseUpdate();
	}
}

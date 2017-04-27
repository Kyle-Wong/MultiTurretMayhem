using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : Enemy {

	// Use this for initialization
	void Start () {
        baseHP = 1;
        curHP = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    new void onPlayerHit()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayerHit();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void death()
    {
        Destroy(this.gameObject);
    }
    public void doDamage()
    {
        //ToDo: do damage to player
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turret")) //ToDo: check if this is actually the tag for the player
        {
            doDamage();
            death();
        }
    }
}

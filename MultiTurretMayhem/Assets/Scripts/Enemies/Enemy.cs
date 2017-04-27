using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int baseHP;
    public int curHP;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void onPlayerHit()
    {
        //Hide this with subclass functionality
    }
    public void doDamage(int damage)
    {
        //ToDo: do damage to player turrets, value passed in by child enemy
    }
    public void takeDamage()
    {
        //ToDo: take damage from player turrets

    }
    public void die()
    {
        Destroy(gameObject);
    }
}

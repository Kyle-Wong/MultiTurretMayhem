using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public int healthAdd;
    public int bombAdd;
    private gameController ctrl;

    void Awake()
    {
        ctrl = GameObject.Find("GameController").GetComponent<gameController>();
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void apply()
    {
        ctrl.bombs += bombAdd;
        ctrl.setHealth(ctrl.getHealth() + healthAdd);
        Destroy(gameObject);
    }
}

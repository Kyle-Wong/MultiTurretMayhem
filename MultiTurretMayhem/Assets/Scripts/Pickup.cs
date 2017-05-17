using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public int healthAdd;
    public int bombAdd;
    private const int MAX_HEALTH = 8;
    private const int MAX_BOMBS = 5;
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
        if(ctrl.bombs < MAX_BOMBS)
            ctrl.bombs += bombAdd;
        if(ctrl.getHealth() < MAX_HEALTH)
            ctrl.setHealth(ctrl.getHealth() + healthAdd);
        Destroy(gameObject);
    }
}

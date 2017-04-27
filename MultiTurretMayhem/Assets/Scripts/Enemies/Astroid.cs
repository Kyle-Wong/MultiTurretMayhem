using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : Enemy {

	// Use this for initialization
	void Start () {
        baseStart();
        baseHP = 1;
        curHP = 1;
	}
	
	// Update is called once per frame
	void Update () {
        baseUpdate();
	}
    

}

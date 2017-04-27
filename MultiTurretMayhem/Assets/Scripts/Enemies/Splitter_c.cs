using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter_c : Enemy {

    // Use this for initialization
    void Start()
    {
        baseStart();
        baseHP = 1;
        curHP = 1;
        launchAway();
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
    }
   
    void launchAway()
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
        this.GetComponent<Rigidbody2D>().AddForce(-toPlayer * 5.0f);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter_c : Enemy {

    // Use this for initialization
    void Start()
    {
        baseHP = 1;
        curHP = 1;
        launchAway();
    }

    // Update is called once per frame
    void Update()
    {

    }
    new void onPlayerHit()
    {
        Destroy(this.gameObject);
    }
    void launchAway()
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
        this.GetComponent<Rigidbody2D>().AddForce(-toPlayer * 5.0f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            onPlayerHit();
        }
    }
}

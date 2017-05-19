using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Enemy
{
    public float tanSpeed = 10;
    private Transform player;
    private Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        baseStart();
        baseHP = 1;
        curHP = 1;
        GetComponent<Simple_Movement>().tanVelocity = HelperFunctions.friendlySqrt(Random.Range(-Mathf.Pow(tanSpeed, 2), Mathf.Pow(tanSpeed, 2)));
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        if (!isDead)
            transform.rotation = Quaternion.LookRotation(Vector3.back, rb.velocity);
    }
}

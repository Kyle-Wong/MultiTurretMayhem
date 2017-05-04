using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Enemy
{
    public float tanSpeed = 10;

    // Use this for initialization
    void Start()
    {
        baseStart();
        baseHP = 1;
        curHP = 1;
        Debug.Log(tanSpeed);
        GetComponent<Simple_Movement>().tanVelocity = HelperFunctions.friendlySqrt(Random.Range(-Mathf.Pow(tanSpeed, 2), Mathf.Pow(tanSpeed, 2)));
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
    }
}

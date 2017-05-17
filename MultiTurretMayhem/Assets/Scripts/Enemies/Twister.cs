using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Enemy
{
    public float tanSpeed = 10;
    private Transform player;
    private int direction;
    // Use this for initialization
    void Start()
    {
        baseStart();
        baseHP = 1;
        curHP = 1;
        GetComponent<Simple_Movement>().tanVelocity = HelperFunctions.friendlySqrt(Random.Range(-Mathf.Pow(tanSpeed, 2), Mathf.Pow(tanSpeed, 2)));
        direction = GetComponent<Simple_Movement>().tanVelocity > 0 ? 0 : 1;
        GetComponentInChildren<SpriteRenderer>().transform.Rotate(0,0,180 * direction);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        transform.rotation = Quaternion.LookRotation(Vector3.back, transform.position - player.position);
    }
}

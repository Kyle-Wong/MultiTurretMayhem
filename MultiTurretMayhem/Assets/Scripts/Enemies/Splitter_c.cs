using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Splitter_c : Enemy
{

    private Transform playerTransform;
    private bool launching;
    private bool goingIn;
    private int frameInvincibility = 5; //so small dudes don't die from the original shot
    public float maxDirVelocity;
    public float closeInitialDirVelocity;
    public float farInitialDirVelocity;
    public float distanceThreshold;
    public float decelerationFactor;
    public float acceleration;
    public AudioClip splitSound;
    public int ID;

    // Use this for initialization
    void Start()
    {
        baseStart();
        playerTransform = GameObject.FindGameObjectWithTag("Turret").transform;
        launchAway();
        baseHP = 1;
        curHP = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            baseUpdate();
            if (frameInvincibility > 0) { frameInvincibility--; }
            if (launching)
            {
                Vector2 curVelocity = GetComponent<Rigidbody2D>().velocity;
                curVelocity = GetComponent<Rigidbody2D>().velocity = curVelocity * decelerationFactor * (1-Time.deltaTime); //Deceleration
                if (Mathf.Abs(curVelocity.x) <= .08f || Mathf.Abs(curVelocity.y) <= .08f) //Max velocity going away from turrets
                {
                    launching = false;
                    GetComponent<Rigidbody2D>().velocity = (playerTransform.position - transform.position).normalized * .08f; //Initial speed
                    goingIn = true;
                }
            }
            if (goingIn)
            {
                Vector2 curVelocity = GetComponent<Rigidbody2D>().velocity;
                curVelocity = GetComponent<Rigidbody2D>().velocity = curVelocity + (Vector2)(playerTransform.position - transform.position).normalized * acceleration * Time.deltaTime; //Acceleration
                if (Mathf.Abs(curVelocity.x) >= maxDirVelocity || Mathf.Abs(curVelocity.y) >= maxDirVelocity) //Max velocity going back into the turrets
                {
                    goingIn = false;
                }
            }
        } else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public override void takeDamage(int damage, Color deathColor)
    {
        if (frameInvincibility == 0)
        {
            curHP -= damage;
            if (curHP <= 0)
            {
                die(deathColor,false);
            }
        }

    }
    private void launchAway()
    {
        Vector3 launch = launchDir();
        if(Vector3.Distance(transform.position,playerTransform.position) < distanceThreshold)
        {
            GetComponent<Rigidbody2D>().velocity = launch * closeInitialDirVelocity;
        } else
        {
            GetComponent<Rigidbody2D>().velocity = launch * farInitialDirVelocity;
        }
    }

    private Vector3 launchDir()
    {
        if (ID == 0) // the one that goes left
        {
            Vector3 awayFromPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 ortho = new Vector3();
            ortho.x = awayFromPlayer.y;
            ortho.y = -awayFromPlayer.x;
            Vector3 avg = new Vector3(ortho.x - awayFromPlayer.x, ortho.y - awayFromPlayer.y, 1);
            launching = true;
            return avg;
        }
        else if (ID == 1) // the one that goes straight
        {
            Vector3 awayFromPlayer = -(playerTransform.position - transform.position).normalized;
            launching = true;
            return awayFromPlayer * 2;
        }
        else // the one that goes right
        {
            Vector3 awayFromPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 ortho = new Vector3();
            ortho.x = awayFromPlayer.y;
            ortho.y = -awayFromPlayer.x;
            Vector3 avg = new Vector3(ortho.x + awayFromPlayer.x, ortho.y + awayFromPlayer.y, 1);

            launching = true;
            return -avg;
        }
    }
}

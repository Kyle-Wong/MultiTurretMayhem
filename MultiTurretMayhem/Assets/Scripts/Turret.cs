﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public enum turretSide
    {
        left,
        right
    }
    public turretSide side;
    public float radius;
    public float minRotSpeed;                   //minimum speed on input
    public float maxRotSpeed;                   //maximum speed reached after accelerationDuration seconds
    public float accelerationDuration;          //time to reach max speed;
    private float accelTimer;

    public float startDirection;                //starting direction
    public string moveLeft;                     //String key input
    public string moveRight;                    //string key input
    public string fire;                         //string key input
    public float fireRate;                      //seconds between shots
    public float damage = 100;                  //damage per hit
    public GameObject laser;                    //laser gameobject
    public Vector2 angleRange;

    private float charge;
    private SpriteRenderer laserSprite;
    private ColorLerp laserColor;               //color of laser
    public bool inputDisabled = false;          //player input is/is not disabled
    void Awake()
    {
        laserSprite = laser.GetComponent<SpriteRenderer>();
        laserColor = laser.GetComponent<ColorLerp>();
    }

    // Use this for initialization
    void Start()
    {
        
        transform.rotation = Quaternion.AngleAxis(startDirection, Vector3.forward);
        transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);

        charge = 1 / fireRate;

        laserSprite.color = Color.clear;
        laserColor.duration = 1 / fireRate;
        accelTimer = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            if (Input.GetKey(moveLeft))
            {
                if (accelTimer < accelerationDuration)
                    accelTimer += Time.deltaTime;
                transform.Rotate(0, 0, Mathf.Lerp(minRotSpeed,maxRotSpeed,accelTimer/accelerationDuration) * Time.deltaTime);
                transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);
            }
            else if (Input.GetKey(moveRight))
            {
                if(accelTimer < accelerationDuration)
                    accelTimer += Time.deltaTime;
                transform.Rotate(0, 0, -Mathf.Lerp(minRotSpeed, maxRotSpeed, accelTimer / accelerationDuration) * Time.deltaTime);
                transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);
            } else
            {
                accelTimer = 0;
            }

            if (Input.GetKeyDown(fire))
            {
                if (charge >= 1 / fireRate)
                    Fire();
            }
            else if (charge < 1 / fireRate)
            {
                charge += Time.deltaTime;
            }
        }
        
    }

    private void Fire()
    {
        RaycastHit2D[] toKill = Physics2D.RaycastAll(transform.position, HelperFunctions.lineVector(transform.rotation.eulerAngles.z), 20);
        foreach (RaycastHit2D e in toKill)
            if (e.collider.CompareTag("Enemy"))
            {
                Enemy enemy = e.collider.gameObject.GetComponent<Enemy>();
                if(enemy.isOnScreen() && !enemy.invincible)
                {
                    enemy.takeDamage(100,laserColor.startColor);
                }
            }

        charge = 0;

        laser.transform.position = transform.position + HelperFunctions.lineVector(transform.rotation.eulerAngles.z, laser.transform.localScale.x * 3);
        laser.transform.rotation = transform.rotation;
        laserColor.startColorChange();
    }
}

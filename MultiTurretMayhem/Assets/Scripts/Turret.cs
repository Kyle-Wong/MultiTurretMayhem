using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public enum turretSide
    {
        left,
        right
    }
    public enum TurretRestriction
    {
        no_restriction,
        left,
        right
    }
    public turretSide side;
    public TurretRestriction restriction = TurretRestriction.no_restriction;
    public float radius;
    public float minRotSpeed;                   //minimum speed on input
    public float maxRotSpeed;                   //maximum speed reached after accelerationDuration seconds
    public float accelerationDuration;          //time to reach max speed;
    private float accelTimer;
    public Color particleColor;

    public float startDirection;                //starting direction
    public string moveLeft;                     //String key input
    public string moveRight;                    //string key input
    public string fire;                         //string key input
    public float fireRate;                      //seconds between shots
    public float damage = 100;                  //damage per hit
    public GameObject laser;                    //laser gameobject
    public Vector2 angleRange;
    public AudioClip fireSound;
    public AudioClip lowerMultiplierSound;
    public AudioClip higherMultiplierSound;

    private float charge;
    private SpriteRenderer laserSprite;
    private ColorLerp laserColor;               //color of laser
    public bool inputDisabled = false;          //player input is/is not disabled
    private gameController ctrl;
    private AudioSource _audioSource;
    private AudioSource _multiplierSource;
    void Awake()
    {
        laserSprite = laser.GetComponent<SpriteRenderer>();
        laserColor = laser.GetComponent<ColorLerp>();
        ctrl = GameObject.Find("GameController").GetComponent<gameController>();
        _audioSource = GetComponents<AudioSource>()[0];
        _multiplierSource = GetComponents<AudioSource>()[1];
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

        _audioSource.clip = fireSound;
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
                switch (restriction)
                {
                    //If out of bounds, counter-rotate
                    case (TurretRestriction.left):
                        if (transform.position.x > 0 && transform.position.y < 0)
                            transform.Rotate(0, 0, -maxRotSpeed * Time.deltaTime);
                        break;
                    case (TurretRestriction.right):
                        if (transform.position.x < 0 && transform.position.y > 0)
                            transform.Rotate(0, 0, -maxRotSpeed * Time.deltaTime);
                        break;
                    case (TurretRestriction.no_restriction):
                        break;
                }
                transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);
            }
            else if (Input.GetKey(moveRight))
            {
                if(accelTimer < accelerationDuration)
                    accelTimer += Time.deltaTime;
                transform.Rotate(0, 0, -Mathf.Lerp(minRotSpeed, maxRotSpeed, accelTimer / accelerationDuration) * Time.deltaTime);
                switch (restriction)
                {
                    //If out of bounds, counter-rotate
                    case (TurretRestriction.left):
                        if(transform.position.x > 0 && transform.position.y > 0)
                            transform.Rotate(0, 0, maxRotSpeed * Time.deltaTime);
                        break;
                    case (TurretRestriction.right):
                        if (transform.position.x < 0 && transform.position.y < 0)
                            transform.Rotate(0, 0, maxRotSpeed * Time.deltaTime);
                        break;
                    case (TurretRestriction.no_restriction):
                        break;
                }
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
        HelperFunctions.playSound(ref _audioSource, fireSound);
        RaycastHit2D[] toKill = Physics2D.RaycastAll(transform.position, HelperFunctions.lineVector(transform.rotation.eulerAngles.z), 20);
        int enemiesHit = 0;
        foreach (RaycastHit2D e in toKill)
        {
            if (e.collider.CompareTag("Enemy"))
            {
                Enemy enemy = e.collider.gameObject.GetComponent<Enemy>();
                if (enemy.isOnScreen() && !enemy.invincible)
                {
                    enemy.takeDamage(100, laserColor.startColor);
                    ParticleSystem.MainModule enemyParticles = enemy.GetComponentInChildren<ParticleSystem>().main;
                    enemyParticles.startColor = particleColor;
                    ++enemiesHit;
                }
            }
            else if (e.collider.CompareTag("Pickup"))
            {
                e.collider.GetComponent<Pickup>().apply();
            }
        }

        if (ctrl.survival)
        {
            if (enemiesHit == 0)
                HelperFunctions.playSound(ref _multiplierSource, lowerMultiplierSound);
            else if (enemiesHit > 1)
                HelperFunctions.playSound(ref _multiplierSource, higherMultiplierSound);
            ctrl.changeMultiplier(enemiesHit);
        }

        charge = 0;
        
        laser.transform.position = transform.position + HelperFunctions.lineVector(transform.rotation.eulerAngles.z, laser.transform.localScale.x * 0.5f);

        laser.transform.rotation = transform.rotation;
        laserColor.startColorChange();
    }
    public void setRestriction(int x)
    {
        restriction = (TurretRestriction)x;
    }

    
}

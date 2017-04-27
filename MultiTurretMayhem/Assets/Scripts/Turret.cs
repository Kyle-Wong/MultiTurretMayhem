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
    public turretSide side;
    public float radius;
    public float degreesPerSecond;
    public float startDirection;
    public string moveLeft;
    public string moveRight;
    public string fire;
    public float fireRate;
    public GameObject laser;

    private float charge;
    private SpriteRenderer laserSprite;
    private ColorLerp laserColor;

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
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(moveLeft))
        {
            transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime);
            transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);
        }
        else if (Input.GetKey(moveRight))
        {
            transform.Rotate(0, 0, -degreesPerSecond * Time.deltaTime);
            transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);
        }

        if (Input.GetKeyDown(fire))
        {
            if (charge >= 1 / fireRate)
                Fire(); 
        }
        else if(charge < 1 / fireRate)
        {
            charge += Time.deltaTime;
        }
    }

    private void Fire()
    {
        RaycastHit2D[] toKill = Physics2D.RaycastAll(transform.position, HelperFunctions.lineVector(transform.rotation.eulerAngles.z), 20);
        foreach (RaycastHit2D e in toKill)
            if (e.collider.CompareTag("Enemy"))
                Destroy(e.collider.gameObject);

        charge = 0;

        laser.transform.position = transform.position + HelperFunctions.lineVector(transform.rotation.eulerAngles.z, laser.transform.localScale.x * 3);
        laser.transform.rotation = transform.rotation;
        laserColor.startColorChange();
    }
}

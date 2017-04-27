using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float radius;
    public float degreesPerSecond;
    public float startDirection;
    public string moveLeft;
    public string moveRight;
    public string fire;
    public float fireRate;

    private float charge;

    // Use this for initialization
    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(startDirection, Vector3.forward);
        transform.position = HelperFunctions.lineVector(transform.rotation.eulerAngles.z, radius);

        charge = 1 / fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(charge);

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
            {
                RaycastHit2D[] toKill = Physics2D.RaycastAll(transform.position, HelperFunctions.lineVector(transform.rotation.eulerAngles.z), 20);
                foreach (RaycastHit2D e in toKill)
                    if (e.collider.CompareTag("Enemy"))
                        Destroy(e.collider.gameObject);

                Debug.DrawRay(transform.position, HelperFunctions.lineVector(transform.rotation.eulerAngles.z, 20), Color.red, 0.2f);

                charge = 0;
            }
            
        }
        else if(charge < 1 / fireRate)
        {
            charge += Time.deltaTime;
        }
    }
}

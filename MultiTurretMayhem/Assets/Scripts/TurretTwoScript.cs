using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTwoScript : MonoBehaviour
{
    public float radius;
    public float degreesPerSecond;

    private Vector3 v;
    private Quaternion rotate;
    RaycastHit2D[] hits;

    // Use this for initialization
    void Start()
    {
        radius = -radius;
        v = new Vector3(radius, 0, 0);
        transform.position = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            rotate = Quaternion.AngleAxis(-(degreesPerSecond * Time.deltaTime), Vector3.forward);
            v = rotate * v;
            transform.rotation *= rotate;
            transform.position = Vector3.zero + v;
        } else if (Input.GetKey("d"))
        {
            rotate = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward);
            v = rotate * v;
            transform.rotation *= rotate;
            transform.position = Vector3.zero + v;
        }

        if (Input.GetKeyDown("w"))
        {
            hits = Physics2D.RaycastAll(transform.position, transform.right + transform.position);

            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.gameObject.name);
            }
            Debug.DrawRay(transform.position, transform.right + transform.position, Color.red, 100f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantTranslation : MonoBehaviour {
    // Use this for initialization
    public bool playOnStartUp = true;
    public float direction = 0; // in degrees
    public float speed = 0;
    private Vector3 velocity;
    void Start()
    {
        velocity = new Vector3(Mathf.Cos((float)(direction * (Mathf.PI / 180))),
            Mathf.Sin((float)(direction * (Mathf.PI / 180))));
        velocity = velocity.normalized * speed;
        if (playOnStartUp)
        {
            startMovement();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator move()
    {
        while (true)
        {
            transform.Translate(velocity * Time.deltaTime);
            yield return null;
        }
    }
    public void startMovement()
    {
        StartCoroutine(move());
    }
    public void setVelocity(float newDirection, float newSpeed)
    {
        direction = newDirection;
        speed = newSpeed;
        velocity = new Vector3(Mathf.Cos((float)(direction * (Mathf.PI / 180))),
                    Mathf.Sin((float)(direction * (Mathf.PI / 180))));
        velocity = velocity.normalized * speed;
    }
}

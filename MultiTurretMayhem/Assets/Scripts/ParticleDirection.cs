using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDirection : MonoBehaviour {
    public bool fromCenter = true;
    public float range;
    public float minSpeed;
    public float width;
    public float duration;
    public float direction;
    private ParticleSystem _particleSystem;

    void Awake () {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void Start () {
        GetComponent<DelAfterTime>().lifeTime = duration;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Emit ()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, HelperFunctions.lineVector(direction));

        ParticleSystem.ShapeModule shape = _particleSystem.shape;
        ParticleSystem.MainModule main = _particleSystem.main;
        shape.radius = width;
        main.startLifetime = duration;

        if (fromCenter)
        {
            main.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, range / duration);
        }
        else
        {
            transform.position += HelperFunctions.lineVector(direction, range);
            main.startSpeed = new ParticleSystem.MinMaxCurve(-range / duration, -minSpeed);
        }

        _particleSystem.Play();
    }
}

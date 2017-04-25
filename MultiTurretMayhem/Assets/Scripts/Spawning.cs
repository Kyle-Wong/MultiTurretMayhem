using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {
    public float offset;                //Where in relation to spawner object (horizontally) this spawner is
    public List<GameObject> objects;    //Which objects to spawn
    public List<float> chances;         //Which probabilties (respectively) at which the objects spawn
    public float generalRate;           //The starting rate at which objects generally spawn (objects per minute)
    public float deltaGeneralRate;      //The change over time in which objects generally spawn (objects per minute squared)
    public float range;                 //The vertical range in which objects can spawn in

    private Targetting targetter;
    private float spawnTimer = 0.0f;

    void Awake()
    {
        targetter = GetComponentInParent<Targetting>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(offset, 0);

        spawnTimer += Time.deltaTime;

        generalRate += deltaGeneralRate * Time.deltaTime / 60; // Rate changes over time

        if (spawnTimer > 60 / generalRate) // Spawn object
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, Random.Range(-1.0f, 1.0f) * range);
            Instantiate(pickRandom(), spawnPosition, Quaternion.LookRotation(Vector3.forward, targetter.target.position - spawnPosition));
            spawnTimer = 0;
        }
	}

    //private float posOrNeg(float f) // Return a positive or negative version of the float randomly
    //{
    //    if (Random.Range(0, 2) == 1)
    //        return -f;
    //    return f;
    //}

    private GameObject pickRandom() // Pick a random object from the objects list using the chances list
    {
        return objects[HelperFunctions.randomIndex(chances)];
    }
}

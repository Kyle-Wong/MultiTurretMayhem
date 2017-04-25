using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {
    public List<GameObject> objects;
    public List<float> chances;
    public float generalRate;
    public float deltaGeneralRate;
    public float range;

    private float spawnTimer = 0.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        spawnTimer += Time.deltaTime;

        generalRate += deltaGeneralRate * Time.deltaTime / 60;

        if (spawnTimer > 60 / generalRate)
        {
            Vector3 spawnPosition = transform.position + new Vector3(posOrNeg(range), Random.Range(-1.0f, 1.0f) * range * 9 / 16);
            Instantiate(pickRandom(), spawnPosition, Quaternion.LookRotation(Vector3.forward, transform.position - spawnPosition));
            spawnTimer = 0;
        }
	}

    private float posOrNeg(float f)
    {
        if (Random.Range(0, 2) == 1)
            return -f;
        return f;
    }

    private GameObject pickRandom()
    {
        return objects[HelperFunctions.randomIndex(chances)];
    }
}

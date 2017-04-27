using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy {

    public GameObject splitterChild;
    public int splittersToSpawn = 3;

    private float deltaDistance = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    new void onPlayerHit()
    {
        for (int i = 0; i < splittersToSpawn; i++)
        {
            Instantiate(splitterChild, generateRandomDistance() , Quaternion.identity);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            onPlayerHit();
        }
    }
    Vector3 generateRandomDistance()
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
        float newX = toPlayer.x + Random.Range(-1, 1) * deltaDistance;
        float newY = toPlayer.y + Random.Range(-1, 1) * deltaDistance;
        return new Vector3(newX, newY, 1f);

    }

}

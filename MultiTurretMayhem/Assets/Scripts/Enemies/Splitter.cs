using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy {

    public GameObject splitterChild;
    public int splittersToSpawn = 3;

    private float deltaDistance = 2;

	// Use this for initialization
	void Start () {
        baseStart();
	}
	
	// Update is called once per frame
	void Update () {
        baseUpdate();
	}
    public override void takeDamage(int damage, Color deathColor)
    {
        curHP -= damage;
        if(curHP <= 0)
        {
            for (int i = 0; i < splittersToSpawn; i++)
            {
                Instantiate(splitterChild, generateRandomDistance(), Quaternion.identity);
            }
            die(deathColor, false);
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

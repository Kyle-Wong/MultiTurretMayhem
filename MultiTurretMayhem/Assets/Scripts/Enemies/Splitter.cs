using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{

    public GameObject splitterChild;
    public int splittersToSpawn = 3;
    private float deltaDistance = 5;

    public AudioClip splitSound;
    // Use this for initialization
    void Start()
    {
        baseStart();
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        
    }
    public override void takeDamage(int damage, Color deathColor)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            for (int i = 0; i < splittersToSpawn; i++)
            {
                GameObject splitter = (GameObject)Instantiate(splitterChild, gameObject.transform.position, Quaternion.identity);
                splitter.GetComponent<Splitter_c>().ID = i;
            }
            HelperFunctions.playSound(ref _audioSource, splitSound);
            die(deathColor, false);
        }

    }
    Vector3 generateRandomDistance()
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
        float newX = toPlayer.x + Random.Range(-1f, 1f) * deltaDistance;
        float newY = toPlayer.y + Random.Range(-1f, 1f) * deltaDistance;
        return new Vector3(newX, newY, 1f);
    }
    void launchAway(GameObject splitter)
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
    }

}
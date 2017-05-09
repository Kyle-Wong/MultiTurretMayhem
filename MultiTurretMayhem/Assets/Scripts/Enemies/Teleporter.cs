using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Enemy {
    public float minDistance;
    public float speedMultiplierPostTeleport = 1;
    public Color postTeleportColor;
    private Transform target;
    public AudioClip teleportSound;

    //void Awake()
    //{
    //    target = GameObject.Find("Spawner").transform;
    //}

    // Use this for initialization
    void Start ()
    {
        baseStart();
        baseHP = 200;
        curHP = 200;
        target = GameObject.Find("Spawner").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        baseUpdate();
	}

    public override void takeDamage(int damage, Color deathColor)
    {
        base.takeDamage(damage, deathColor);
        if (curHP > 0)
        {
            teleport();
            GetComponentInChildren<ColorOscillationWithSprites>().stopColorChange();
            GetComponent<Simple_Movement>().dirVelocity *= speedMultiplierPostTeleport;
            GetComponentInChildren<SpriteRenderer>().color = postTeleportColor;
        }
    }

    public void teleport()
    {
        dropEffect(new Color(0.7f,0.7f,0.7f), new Color(0,0,0,0));
        HelperFunctions.playSound(ref _audioSource, teleportSound);

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < minDistance)
            transform.position = target.position + HelperFunctions.lineVector(HelperFunctions.directionBetween(transform.position, target.position), minDistance);
        else
            transform.position = target.position + HelperFunctions.lineVector(HelperFunctions.directionBetween(transform.position, target.position), distance);

        transform.Rotate(0, 0, 180);

        dropEffect(new Color(0,0,0,0),new Color(0.7f,0.7f,0.7f,0.7f));
    }
    private void dropEffect(Color startColor, Color endColor)
    {
        GameObject effect = (GameObject)Instantiate(Resources.Load("TeleportEffect"));
        effect.transform.position = transform.position;
        effect.GetComponent<ColorLerp>().setColors(startColor, endColor);
    }
}

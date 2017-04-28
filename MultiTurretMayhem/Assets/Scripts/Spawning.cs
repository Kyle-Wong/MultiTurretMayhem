using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {
    public Transform target;            //The target to aim at
    public List<GameObject> objects;    //Which objects to spawn
    public List<float> chances;         //Which probabilties (respectively) at which the objects spawn
    public float leftRate;              //The starting rate at which objects generally spawn on the left side (objects per minute)
    public float deltaLeftRate;         //The change over time in which objects generally spawn on the left side(objects per minute squared)
    public List<Vector2> leftAngles;          //The angle range at which the object can spawn on the left side (degrees)
    public float rightRate;             //The starting rate at which objects generally spawn on the right side (objects per minute)
    public float deltaRightRate;        //The change over time in which objects generally spawn on the right side(objects per minute squared)
    public List<Vector2> rightAngles;         //The angle range at which the object can spawn on the right side (degrees)
    public float distance;              //The distance at which objects spawn from

    private float spawnTimer = 0.0f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        spawnTimer += Time.deltaTime;

        leftRate += deltaLeftRate * Time.deltaTime / 60; //Rate changes over time
        rightRate += deltaRightRate * Time.deltaTime / 60; //Rate changes over time

        float generalRate = leftRate + rightRate;

        if (spawnTimer > 60 / generalRate)
            spawnObject();
	}

    //private float posOrNeg(float f) //Return a positive or negative version of the float randomly
    //{
    //    if (Random.Range(0, 2) == 1)
    //        return -f;
    //    return f;
    //}

    private GameObject pickRandom() //Pick a random object from the objects list using the chances list
    {
        return objects[HelperFunctions.randomIndex(chances)];
    }

    private void spawnObject() //Spawn a game object
    {
        float spawnAngle = 0.0f;
        Vector2 angleRange;
        if (Random.Range(0.0f, leftRate + rightRate) < leftRate)
            angleRange = leftAngles[HelperFunctions.randomIndex(leftAngles)];
        else
            angleRange = rightAngles[HelperFunctions.randomIndex(rightAngles)];
        spawnAngle = Random.Range(angleRange.x, angleRange.y);

        Vector3 spawnPosition = transform.position + HelperFunctions.lineVector(spawnAngle, distance);
        Instantiate(pickRandom(), spawnPosition, Quaternion.LookRotation(Vector3.forward, target.position - spawnPosition));
        spawnTimer = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {
    public List<GameObject> objects;    //Which objects to spawn
    public List<float> leftChances;         //Which probabilties (respectively) at which the objects spawn
    public List<float> rightChances;
    public float leftRate;              //The starting rate at which objects generally spawn on the left side (objects per minute)
    public float deltaLeftRate;         //The change over time in which objects generally spawn on the left side(objects per minute squared)
    public float deltaLeftAngle;
    public float rightRate;             //The starting rate at which objects generally spawn on the right side (objects per minute)
    public float deltaRightRate;        //The change over time in which objects generally spawn on the right side(objects per minute squared)
    public float deltaRightAngle;
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

    private GameObject pickRandom(List<float> chances) //Pick a random object from the objects list using the chances list
    {
        return objects[HelperFunctions.randomIndex(chances)];
    }
    /*
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
    */
    private void spawnObject() //Spawn a game object
    {
        const int LEFT = -1;
        const int RIGHT = 1;
        int side = 0;
        float spawnAngle = 0.0f;
        if (Random.Range(0.0f, leftRate + rightRate) < leftRate)
        {
            spawnAngle = 180 + Random.Range(-deltaLeftAngle / 2, deltaLeftAngle / 2);
            side = LEFT;
        }
        else
        {
            spawnAngle = Random.Range(-deltaRightAngle / 2, deltaRightAngle / 2);
            side = RIGHT;
        }

        Vector3 spawnPosition = transform.position + HelperFunctions.lineVector(spawnAngle, distance);
        if(side == LEFT)
            Instantiate(pickRandom(leftChances), spawnPosition, Quaternion.LookRotation(Vector3.forward, Vector3.zero-spawnPosition));
        if(side == RIGHT)
            Instantiate(pickRandom(rightChances), spawnPosition, Quaternion.LookRotation(Vector3.forward, Vector3.zero - spawnPosition));
        spawnTimer = 0;
    }
}

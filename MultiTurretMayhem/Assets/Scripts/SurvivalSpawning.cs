using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSpawning : MonoBehaviour {
    public List<GameObject> objects;
    public List<float> spawnChance;

    public List<GameObject> uncommonObjects;
    public List<GameObject> commonObjects;

    private List<GameObject> tempCommonObjects;

    public float uncommonSpawnRate;
    public float commonSpawnRate;

    public float setTimer = 60f;
    public float timer = 60f;

    private int uncommonObjectIndex;
    private int uncommonObjectIndex2;

    Random random = new Random();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            HelperFunctions.fillList<float>(ref spawnChance, 0);

            spawnChance[objects.IndexOf(commonObjects[Random.Range(0, commonObjects.Count)])] = commonSpawnRate; // Finds a random uncommon object and sets its spawn rate to the uncommon spanwn rate

            uncommonObjectIndex = Random.Range(0, uncommonObjects.Count);

            spawnChance[objects.IndexOf(uncommonObjects[uncommonObjectIndex])] = uncommonSpawnRate; // Sets spawn rate of common object to the common spanwn rate

            do  // checks if second random common object is the same as the first
            {
                uncommonObjectIndex2 = Random.Range(0, uncommonObjects.Count);
            } while (uncommonObjectIndex == uncommonObjectIndex2);

            spawnChance[objects.IndexOf(uncommonObjects[uncommonObjectIndex2])] = uncommonSpawnRate; // Sets spawn Rate of common object to common spawn rate

            GetComponent<Spawning>().leftChances = spawnChance;
            GetComponent<Spawning>().rightChances = spawnChance;

            timer = setTimer;
        }
	}
}

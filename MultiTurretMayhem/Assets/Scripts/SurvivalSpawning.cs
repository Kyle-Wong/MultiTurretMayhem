using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSpawning : MonoBehaviour {
    public List<GameObject> objects;
    public List<float> spawnChance;

    public List<GameObject> commonObjects;
    public List<GameObject> uncommonObjects;

    public List<float> timerList;
    public List<float> deltaRate;

    private List<GameObject> tempCommonObjects;

    public float commonSpawnRate;
    public float uncommonSpawnRate;

    public float setTimer = 60f;
    public float timer = 60f;

    public int rateListCounter = 0;
    public float deltaTimer;

    private int uncommonObjectIndex;
    private int uncommonObjectIndex2;

    Random random = new Random();

	// Use this for initialization
	void Start () {
        deltaTimer = timerList[0];
        GetComponent<Spawning>().leftChances = spawnChance;
        GetComponent<Spawning>().rightChances = spawnChance;
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        if (rateListCounter < timerList.Count)
        {
            deltaTimer -= Time.deltaTime;
        }

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

        if (deltaTimer <= 0 && rateListCounter < timerList.Count)
        {
            GetComponent<Spawning>().deltaLeftRate = deltaRate[rateListCounter];
            GetComponent<Spawning>().deltaRightRate = deltaRate[rateListCounter];

            rateListCounter++;
            if (rateListCounter < timerList.Count)
            {
                deltaTimer = timerList[rateListCounter];
            } else
            {
                deltaTimer = int.MaxValue;  //no more changes to spawn rate growth
            }
     
        }
	}
}

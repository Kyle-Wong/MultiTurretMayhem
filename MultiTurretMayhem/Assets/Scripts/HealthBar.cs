using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    // Use this for initialization
    private const int MAX_HEALTH = 8;
    private int health;
    private gameController controller;
    private List<GameObject> healthPips;
    private bool gameIsOver;
	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        health = controller.health;
        healthPips = getHealthPips();
        gameIsOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(health != controller.health)
        {
            health = controller.health;
            updatePips(health);
        }
        if(!gameIsOver && controller.gameIsOver)   //run once when game ends
        {
            gameIsOver = true;
            for(int i = 0; i < healthPips.Count; i++)
            {
                if (healthPips[i].activeSelf)
                {
                    healthPips[i].GetComponent<ColorLerp>().startColorChange();
                    healthPips[i].transform.GetChild(0).gameObject.GetComponent<ColorLerp>().startColorChange();
                }
            }
        }
	}
    private List<GameObject> getHealthPips()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
    private void updatePips(int hp)
    {
        if(hp > MAX_HEALTH)
        {
            print("ERROR: player health exceeds MAX_HEALTH");
        }
        for (int i = 0; i < healthPips.Count; i++)
        {
            healthPips[i].SetActive(false);
            healthPips[i].transform.GetChild(0).gameObject.SetActive(false);

            //turn off all health pips
        }
        for (int i = 0; i < hp; i++)
        {
            healthPips[i].SetActive(true);
            healthPips[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }
}

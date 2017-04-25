using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

    // Use this for initialization
    public Text pointsText;
    public Text timerText;
    public int levelNum;
    private List<Spawning> spawnList;
    private int points;
    public float timeRemaining;
	void Start () {
        spawnList = getSpawners();	
	}
	
	// Update is called once per frame
	void Update () {
        timeRemaining -= Time.deltaTime;
        if(timerText != null)
        {
            timerText.text = "" + (timeRemaining / 60) + ":";
            timerText.text = (timeRemaining / 60 >= 10) ? timerText.text + timeRemaining / 60 : timerText.text + "0"+timeRemaining / 60;
        }
        if(pointsText != null)
        {
            int length = (int)Mathf.Floor(Mathf.Log10(points)) + 1;
            string result = "";
            for(int i = length; i < 5; i++)
            {
                result += 0;
            }
            result += "" + points;
        }
	}
    private List<Spawning> getSpawners()
    {
        List<Spawning> children = new List<Spawning>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject.GetComponent<Spawning>());
        }
        return children;
    }
    public void addPoints(int amount)
    {
        points += amount;
    }
    public int getPoints()
    {
        return points;
    }
    public float time()
    {
        return timeRemaining;
    }
}

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
    private List<GameObject> settingsList;
    private GameSettings currentSettings;
    private int points;
    private float timeRemaining;
	void Start () {
        settingsList = getSettings();
        setLevelSettings(levelNum);
        currentSettings = settingsList[levelNum].GetComponentInChildren<GameSettings>();
        timeRemaining = currentSettings.levelDuration;
        points = 0;

    }
	
	// Update is called once per frame
	void Update () {
        timeRemaining -= Time.deltaTime;
        
        if(timerText != null)
        {
            timerText.text = timeAsString((int)timeRemaining);
        }
        
        if(pointsText != null)
        {
            pointsText.text = pointsAsString(points);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            addPoints(9);
        }
	}
    private List<GameObject> getSettings()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
    private void setLevelSettings(int currentLevel)
    {
        for(int i = 0; i < settingsList.Count; i++)
        {
            if(i != currentLevel)
            {
                settingsList[i].SetActive(false);
            } else
            {
                settingsList[i].SetActive(true);
            }
        }
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
    private string pointsAsString(int score)
    {
        string result = "";
        int length = score.ToString().Length;
        for (int i = length; i < 5; i++)
        {
            result += 0;
        }
        result += "" + score;
        return result;
    }
    private string timeAsString(int seconds)
    {
        string result = "";
        result = "" + (seconds / 60) + ":";
        result +=""+ ((seconds / 60 >= 10) ? ""+seconds / 60 : "0" + seconds / 60);
        return result;
    }
}

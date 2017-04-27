using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

    // Use this for initialization
    public int health;
    public Text pointsText;
    public ProgressBar chargeBar;
    public FTLText chargeText;
    public int levelNum;
    private List<GameObject> settingsList;
    private GameSettings currentSettings;
    private int points;
    private float timeRemaining;
    private float totalDuration;
	void Start () {
        settingsList = getSettings();
        setLevelSettings(levelNum);
        currentSettings = settingsList[levelNum].GetComponentInChildren<GameSettings>();
        timeRemaining = currentSettings.levelDuration;
        totalDuration = timeRemaining;
        points = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        } else
        {
            timeRemaining = 0f;
        }
        
        if(chargeText != null)
        {
            chargeText.setPercent(1 - (timeRemaining / totalDuration));
        }
        if(chargeBar != null)
        {
            chargeBar.setProgress(1-(timeRemaining/totalDuration));
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
    /*
    private string timeAsString(int seconds)
    {
        string result = "";
        result = "" + (seconds / 60) + ":";
        result +=""+ ((seconds / 60 >= 10) ? ""+seconds / 60 : "0" + seconds / 60);
        return result;
    }
    */
    public void setHealth(int x)
    {
        health = x;
    }
    public void damagePlayer(int x)
    {
        health -= x;
    }
    public int getHealth()
    {
        return health;
    }
}

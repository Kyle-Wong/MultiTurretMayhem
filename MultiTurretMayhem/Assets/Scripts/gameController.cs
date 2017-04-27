using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

    // Use this for initialization
    public enum GameState
    {
        beforeGame,
        duringGame,
        gameOver,
        transitionOver
    }
    public int health = 100;
    public int bombs = 3;
    public Text pointsText;
    public ProgressBar chargeBar;
    public FTLText chargeText;
    public ColorLerp shield;
    public FTLJump ftlJump;
    public int levelNum;
    private List<GameObject> settingsList;
    private GameSettings currentSettings;
    private int points;
    public bool survival = false;
    private float timeRemaining;
    private float totalDuration;
    private GameState gameState;
    public float delayBeforeJump;
    public float jumpDuration;
    private float jumpTimer;
    public bool hideMouse = true;

	void Start () {
        settingsList = getSettings();
        setLevelSettings(levelNum);
        currentSettings = settingsList[levelNum].GetComponentInChildren<GameSettings>();
        timeRemaining = currentSettings.levelDuration;
        totalDuration = timeRemaining;
        points = 0;
        gameState = GameState.beforeGame;
        gameState = GameState.duringGame;  //debug, will normally start at beforeGame
        if (hideMouse)
            Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            addPoints(9);
            damagePlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            bombs++;
            dropBomb();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            dropFTLBomb();
        }
        switch (gameState)
        {
            case (GameState.beforeGame):
                break;
            case (GameState.duringGame):
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    timeRemaining = 0f;                                             //Ensure values sent to progress bar
                                                                                    //and progress text do not exceed 100%
                }

                if (chargeText != null)
                {
                    chargeText.setPercent(1 - (timeRemaining / totalDuration));     //update progress bar
                }
                if (chargeBar != null)
                {
                    chargeBar.setProgress(1 - (timeRemaining / totalDuration));     //update text in progress bar
                }
                if (pointsText != null)
                {
                    pointsText.text = pointsAsString(points);                       //display points
                }
                if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    dropBomb();
                }
                if (!survival && timeRemaining <= 0)                    //survival game mode never leaves this state
                {
                    chargeBar.setProgress(1);              
                    dropFTLBomb();                                      //kill all enemies
                    settingsList[levelNum].SetActive(false);            //turn off enemySpawner
                    jumpTimer = 0;                                      //set jump timer to zero for next state
                    gameState = GameState.gameOver;                     //State Transition
                    GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
                    for(int i = 0; i < turrets.Length; i++)
                    {
                        turrets[i].GetComponent<Turret>().inputDisabled = true;             //disable player input
                        turrets[i].GetComponent<ColorLerp>().startColorChange();            //fade turrets to transparent
                        turrets[i].transform.GetChild(0).gameObject.SetActive(false);       //disable targetting lines
                    }
                    
                }
                break;
            case (GameState.gameOver):
                if(delayBeforeJump > 0)
                {
                    delayBeforeJump -= Time.deltaTime;                  //delay before FTL jump
                } else
                {
                    if(jumpTimer == 0)
                    {
                        chargeText.enabled = false;                     //disable automatic color/text changes
                        chargeBar.enabled = false;                      //disable automatic color/text changes
                        ftlJump.startAllStars();                        //start FTL jump
                        chargeText.GetComponent<Text>().text = "Jumping...";
                    }
                    if(jumpTimer < jumpDuration)
                    {
                        jumpTimer += Time.deltaTime;
                        chargeBar.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1-(jumpTimer/jumpDuration),1);
                        //Decrease progress bar during jump
                    } else
                    {
                        ftlJump.stopAllStars();                         //stop FTL jump
                        chargeText.GetComponent<Text>().text = "Jump Complete";
                        gameState = GameState.transitionOver;           //State Transition
                    }
                }
                break;
            case (GameState.transitionOver):
                //display GUI
                break;
        }
	}
    private List<GameObject> getSettings()
    {        
        List<GameObject> children = new List<GameObject>();         //get all children of this gameobject
                                                                    //Each child object has a gameSettings and
                                                                    //Spawner as children
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
    private void setLevelSettings(int currentLevel)
    {
        //  Disable all spawners except the current level's

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
   
    public void setHealth(int x)
    {
        health = x;
    }
    public void damagePlayer(int x)
    {
        health -= x;
        shield.startColorChange();
    }
    public int getHealth()
    {
        return health;
    }
    public void dropBomb()
    {
        if(bombs > 0)
        {
            bombs--;
            GameObject bomb = (GameObject)Instantiate(Resources.Load("Bomb"));
            bomb.transform.position = Vector3.zero;
        }
        
    }
    public void dropFTLBomb()
    {
        GameObject FTLBomb = (GameObject)Instantiate(Resources.Load("FTLBomb"));
        FTLBomb.transform.position = Vector3.zero;
    }
}

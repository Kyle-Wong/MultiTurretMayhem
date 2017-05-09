using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class gameController : MonoBehaviour {

    // Use this for initialization
    public enum GameState
    {
        beforeGame,
        duringGame,
        transition,
        transitionOver,
        survivalDeathState,
        campaignDeathState,
        paused
    }
    //public struct HighScore
    //{
    //    public float score;
    //    public string name;
    //}
    public int health = 8;
    private int maxHealth;
    public int bombs = 3;
    public Text pointsText;
    public ProgressBar chargeBar;
    public FTLText chargeText;
    public ColorLerp shield;
    public FTLJump ftlJump;
    public Text lowHealthText;
    public GameObject tutorialCanvas;
    public GameObject deathCanvas;
    public GameObject levelCompletionCanvas;
    public GameObject pauseCanvas;
    public Transform cam;
    public float screenShakeDuration;
    public float screenShakeMagnitude;
    public int levelNum;
    private List<GameObject> settingsList;
    private GameSettings currentSettings;
    private int points;
    public float lowHealthThreshold;
    public bool survival = false;
    private float timeRemaining;
    private float totalDuration;
    private GameState gameState;
    public float preGameDelay;
    public float delayBeforeJump;
    public float jumpDuration;
    public float delayAfterJump;
    private float jumpTimer;
    public bool hideMouse = true;
    private bool playerIsDead = false;
    public bool gameIsOver = false;
    public float multiplier = 1;
    public Text multiplierText;
    private EventSystem eventSystem;
    private GraphicColorLerp blackPanel;
    public AudioSource shipAudio;
    public AudioClip shipHit;
    public AudioClip shipDanger;
    public AudioClip shipDeath;
    public AudioClip bombSound;
    private List<HighScore> highScores;
	void Awake () {
        maxHealth = health;
        settingsList = getSettings();
        blackPanel = GameObject.Find("BlackPanel").GetComponent<GraphicColorLerp>();
        
        if (!survival)
            levelNum = LevelNumber.getLevel();
        setLevelSettings(levelNum);
        currentSettings = settingsList[levelNum].GetComponentInChildren<GameSettings>();
        timeRemaining = currentSettings.levelDuration;
        totalDuration = timeRemaining;
        points = 0;
        lowHealthText.enabled = false;
        bombs = currentSettings.startingBombs;
        gameState = GameState.beforeGame;
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if (hideMouse)
            Cursor.visible = false;
    }
    void Start()
    {
        if (!LevelNumber.getSkipIntro())
        {
            blackPanel.gameObject.GetComponent<Image>().enabled = true;
            blackPanel.startColorChange();
        }
        else
        {
            blackPanel.gameObject.SetActive(false);
            preGameDelay = 0;
        }
        LevelNumber.setSkipIntro(true);     //main menu intro should no longer be played when returning to it
        shipAudio.clip = shipHit;
        highScores = getHighScores();
    }
    // Update is called once per frame
    void Update() {

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
        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelNumber.setLevel(levelNum + 1);
            SceneManager.LoadScene("Campaign");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            LevelNumber.setLevel(levelNum - 1);
            SceneManager.LoadScene("Campaign");
        }
        
        switch (gameState)
        {
            case (GameState.beforeGame):
                if(preGameDelay > 0)
                {
                    preGameDelay -= Time.deltaTime;
                } else
                {
                    gameState = GameState.duringGame;
                }
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameState = GameState.paused;
                    pauseCanvas.SetActive(true);
                    eventSystem.SetSelectedGameObject(GameObject.Find("ResumeButton"));                               //set default button
                    GameObject.Find("ResumeButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                    Time.timeScale = 0;
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
                multiplierText.text = multiplier.ToString("n1") + " x";
                multiplierText.color = HelperFunctions.colorInterpolation(Color.red, Color.white, (multiplier - 1) / 9);
                if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    dropBomb();
                }
                if (playerIsDead)
                {
                    for (int i = 0; i < currentSettings.enabledUI.Length; i++)
                    {
                        currentSettings.enabledUI[i].SetActive(false);
                    }
                    lowHealthText.enabled = false;
                    gameIsOver = true;
                    GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
                    for (int i = 0; i < turrets.Length; i++)
                    {

                        turrets[i].GetComponent<Turret>().inputDisabled = true;             //disable player input
                        turrets[i].GetComponent<ColorLerp>().startColorChange();            //fade turrets to transparent
                        turrets[i].transform.GetChild(0).gameObject.SetActive(false);       //disable targetting lines
                    }
                    deathCanvas.SetActive(true);
                    if (survival)
                    {
                        gameState = GameState.survivalDeathState;   //change state to game over screen
                        eventSystem.SetSelectedGameObject(GameObject.Find("RestartButton"));
                        GameObject.Find("RestartButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                    } else
                    {
                        gameState = GameState.campaignDeathState;   //change state to campaign game over screen
                        eventSystem.SetSelectedGameObject(GameObject.Find("RestartButton"));
                        GameObject.Find("RestartButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                    }
                }
                
                if (!survival && timeRemaining <= 0)                    //survival game mode never leaves this state
                {
                    lowHealthText.enabled = false;
                    gameIsOver = true;
                    for(int i = 0; i < currentSettings.enabledUI.Length; i++)
                    {
                        currentSettings.enabledUI[i].SetActive(false);
                    }
                    chargeBar.setProgress(1);              
                    dropFTLBomb();                                      //kill all enemies
                    settingsList[levelNum].SetActive(false);            //turn off enemySpawner
                    jumpTimer = 0;                                      //set jump timer to zero for next state
                    gameState = GameState.transition;                     //State Transition
                    GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
                    for(int i = 0; i < turrets.Length; i++)
                    {
                        turrets[i].GetComponent<Turret>().inputDisabled = true;             //disable player input
                        turrets[i].GetComponent<ColorLerp>().startColorChange();            //fade turrets to transparent
                        turrets[i].transform.GetChild(0).gameObject.SetActive(false);       //disable targetting lines
                    }
                    
                }
                break;
            case (GameState.transition):

                if (delayBeforeJump > 0)
                {
                    delayBeforeJump -= Time.deltaTime;                  //delay before FTL jump
                }
                else
                {
                    if (jumpTimer == 0)
                    {
                        chargeText.enabled = false;                     //disable automatic color/text changes
                        chargeBar.enabled = false;                      //disable automatic color/text changes
                        ftlJump.startAllStars();                        //start FTL jump
                        chargeText.GetComponent<Text>().text = "Jumping...";
                    }
                    if (jumpTimer < jumpDuration)
                    {
                        jumpTimer += Time.deltaTime;
                        chargeBar.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1 - (jumpTimer / jumpDuration), 1);
                        //Decrease progress bar during jump
                    }
                    else
                    {
                        ftlJump.stopAllStars();                         //stop FTL jump
                        chargeText.GetComponent<Text>().text = "Jump Complete";
                        if(delayAfterJump > 0)
                        {
                            delayAfterJump -= Time.deltaTime;
                        } else
                        {
                            gameState = GameState.transitionOver;           //State Transition
                            levelCompletionCanvas.SetActive(true);
                            eventSystem.SetSelectedGameObject(GameObject.Find("MainMenuButton"));                               //set default button
                            GameObject.Find("MainMenuButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                            if (levelNum >= settingsList.Count)
                            {
                                GameObject.Find("ContinueButton").GetComponent<Button>().interactable = false;      //if on last level, grey out continueButton
                            }
                        }
                    }
                }
                
                break;
            case (GameState.transitionOver):

                //display GUI
                break;
            case (GameState.campaignDeathState):
                
                break;
            case (GameState.survivalDeathState):
                setHighScore(points);

                break;
            case (GameState.paused):
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameState = GameState.duringGame;
                    pauseCanvas.SetActive(false);
                    eventSystem.SetSelectedGameObject(null);
                    Time.timeScale = 1;
                }
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
        if (!playerIsDead)
        {
            health -= x;
            StartCoroutine(cameraShake(screenShakeDuration, screenShakeMagnitude));
        }
        shield.startColor.a = Mathf.Lerp(0f, 1f, health * 1.0f / maxHealth);
        shield.startColorChange();
        if(health*1f/maxHealth < lowHealthThreshold)
        {
            lowHealthText.enabled = true;
            HelperFunctions.playSound(ref shipAudio, shipDanger);
        }
        else if (health * 1f / maxHealth >= lowHealthThreshold)
        {
            HelperFunctions.playSound(ref shipAudio, shipHit);
        }
        else if(health <= 0)
        {
            playerIsDead = true;
            lowHealthText.enabled = false;
            health = 0;
            HelperFunctions.playSound(ref shipAudio, shipDeath);
        }
        
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
            HelperFunctions.playSound(ref shipAudio, bombSound);
        }
        
    }
    public void dropFTLBomb()
    {
        GameObject FTLBomb = (GameObject)Instantiate(Resources.Load("FTLBomb"));
        FTLBomb.transform.position = Vector3.zero;
    }

    public IEnumerator cameraShake (float duration, float magnitude)
    {
        Vector3 originalPos = cam.position;

        for (float elapsed = 0.0f; elapsed < duration; elapsed += Time.deltaTime)
        {
            float x = Random.Range(-magnitude, magnitude) * (1 - elapsed / duration);
            float y = Random.Range(-magnitude, magnitude) * (1 - elapsed / duration);
            cam.position = originalPos + new Vector3(x, y, 0);
            yield return new WaitForEndOfFrame();
        }
        cam.position = originalPos;
    }
    public int getGameState()
    {
        return (int)gameState;
    }
    public void mainMenuButton()
    {
        Time.timeScale = 1;     //in case of returning to main menu from pause screen
        SceneManager.LoadScene("MainMenu");
    }
    public void continueButton()
    {
        StartCoroutine(loadNextLevel());
        eventSystem.SetSelectedGameObject(null);
    }
    public IEnumerator loadNextLevel()
    {
        GameObject.Find("BlackPanel1").GetComponent<GraphicColorLerp>().startColorChange();
        LevelNumber.setSkipIntro(false);
        yield return new WaitForSeconds(1.5f);
        LevelNumber.setLevel(levelNum + 1);
        SceneManager.LoadScene("Campaign");
    }
    public void restartLevelButton()
    {
        SceneManager.LoadScene("Campaign");
    }
    public void restartSurvival()
    {
        SceneManager.LoadScene("Survival");
    }
    public void resumeButton()
    {
        gameState = GameState.duringGame;
        pauseCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        Time.timeScale = 1;
    }
    
    public List<HighScore> getHighScores()
    {
        List<HighScore> result = new List<HighScore>(10);
        HelperFunctions.fillList(ref result, new HighScore());

        for (int i = 0; i < result.Count; ++i)
        {
            string s = "highScore" + i.ToString();
            HighScore temp = result[i];
            temp.name = s;
            temp.score = PlayerPrefs.GetInt(s);
        }
        return result;
    }

    public void setHighScore(int score)
    {
        if (score < highScores[highScores.Count-1].score)
            return;

        HighScore temp = highScores[highScores.Count - 1];
        temp.score = score;
        highScores[highScores.Count - 1] = temp;
        sortHighScores(ref highScores);

        for (int i = 0; i < highScores.Count; ++i)
        {
            string s = "highScore" + i.ToString();
            PlayerPrefs.SetInt(s, highScores[i].score);
        }
        
    }

    public void sortHighScores(ref List<HighScore> hs)
    {
        hs.Sort(delegate (HighScore a, HighScore b) { return a.score - b.score; });
    }
}

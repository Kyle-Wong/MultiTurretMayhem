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
        paused,
        highScore
    }
    public int health = 8;
    private int maxHealth;
    public int bombs = 3;
    private bool bombUsed = false;
    public float setBombCooldown;
    private float bombCooldown;
    public Text pointsText;
    public ProgressBar chargeBar;
    public FTLText chargeText;
    public ColorLerp shield;
    private FTLJump ftlJump;
    public Text lowHealthText;
    public GameObject tutorialCanvas;
    public GameObject deathCanvas;
    public GameObject levelCompletionCanvas;
    public GameObject pauseCanvas;
    public GameObject highScoreCanvas;
    public Transform cam;
    public UIOutline outline;
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
    public float missMultiplier;
    private EventSystem eventSystem;
    private GraphicColorLerp blackPanel;
    public AudioSource shipAudio;
    public AudioSource shipDangerAudio;
    public AudioClip shipHit;
    public AudioClip shipDanger;
    public AudioClip shipDeath;
    public AudioClip bombSound;
    private List<HighScore> highScores;
    private GameObject[] targetList;
    private bool tutorialRunning = false;
    public List<GameObject> dropItems;
    public Vector2 dropTimes;
    public float dropTimer = 0.0f;
    private float timeToDrop;
    public List<float> dropRates;
    private AudioSource musicSource;
    public List<AudioClip> musicList;

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
        musicSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartCoroutine(playSongs());
        LevelNumber.setSkipIntro(true);     //main menu intro should no longer be played when returning to it
        ftlJump = GameObject.FindGameObjectWithTag("StarList").GetComponent<FTLJump>();
        ftlJump.stopAllStars();
        blackPanel.gameObject.GetComponent<Image>().enabled = true;
        if (LevelNumber.getLoadedFromMenu())
        {
            blackPanel.setColors(new Color(0, 0, 0, 1), new Color(0,0,0,0));
        } else
        {
            blackPanel.setColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
            chargeBar.setProgress(0);
            chargeText.setPercent(0);
        }
        blackPanel.startColorChange();
        LevelNumber.setLoadedFromMenu(false);
        shipAudio.clip = shipHit;
        highScores = getHighScores();
        bombCooldown = setBombCooldown;
        
    }
    // Update is called once per frame
    void Update() {
        if (bombUsed)
        {
            bombCooldown -= Time.deltaTime;
            
            if (bombCooldown <= 0)
            {
                bombUsed = false;
                bombCooldown = setBombCooldown;
            }
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
                runPreGame();  //level tutorial logic is in here.  Also handles levels without tutorials
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameState = GameState.paused;
                    pauseCanvas.SetActive(true);
                    eventSystem.SetSelectedGameObject(GameObject.Find("ResumeButton"));                               //set default button
                    GameObject.Find("ResumeButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                    Time.timeScale = 0;
                }
                break;
            case (GameState.duringGame):
                if (survival)
                {
                    StartCoroutine(advanceDropTimer());
                }
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
                if (multiplierText != null)
                {
                    multiplierText.text = multiplier.ToString("n1") + " x";
                    multiplierText.color = HelperFunctions.colorInterpolation(Color.red, Color.white, (multiplier - 1) / 9);
                }
                if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !bombUsed)
                {
                    dropBomb();
                }
                if (playerIsDead)
                {
                    lowHealthText.enabled = false;
                    shipDangerAudio.enabled = false;
                    gameIsOver = true;
                    for (int i = 0; i < currentSettings.enabledUI.Length; i++)
                    {
                        currentSettings.enabledUI[i].SetActive(false);
                    }
                    settingsList[levelNum].SetActive(false);            //turn off enemySpawner
                    GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
                    for (int i = 0; i < turrets.Length; i++)
                    {

                        turrets[i].GetComponent<Turret>().inputDisabled = true;             //disable player input
                        turrets[i].GetComponent<Turret>().fadeTurret();            //fade turrets to transparent
                        turrets[i].transform.Find("TargettingLine").gameObject.SetActive(false);
                    }
                    GameObject ship = GameObject.FindWithTag("Player");
                    ship.transform.Find("Ship").GetComponent<ColorLerp>().startColorChange();
                    ship.GetComponent<ParticleSystem>().Play();
                    Instantiate(Resources.Load("ClearExplosion"), ship.transform.position, Quaternion.identity);
                    deathCanvas.SetActive(true);
                    if (survival)
                    {
                        gameState = GameState.survivalDeathState;   //change state to game over screen
                        eventSystem.SetSelectedGameObject(GameObject.Find("RestartButton"));
                        GameObject.Find("RestartButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                        setHighScore(points);
                    }
                    else
                    {
                        gameState = GameState.campaignDeathState;   //change state to campaign game over screen
                        eventSystem.SetSelectedGameObject(GameObject.Find("RestartButton"));
                        GameObject.Find("RestartButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                    }
                }
                
                if (!survival && timeRemaining <= 0)                    //survival game mode never leaves this state
                {
                    lowHealthText.enabled = false;
                    shipDangerAudio.enabled = false;
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
                        turrets[i].GetComponent<Turret>().fadeTurret();       //fade turrets to transparent
                        turrets[i].transform.Find("TargettingLine").gameObject.SetActive(false);
                        //disable targetting lines
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
                        //Decrease progress bar during jump
                        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                        {
                            jumpTimer = jumpDuration;
                        }
                    }
                    else
                    {
                        gameState = GameState.transitionOver;           //State Transition
                        levelCompletionCanvas.SetActive(true);
                        if (levelNum < 7)
                        {
                            eventSystem.SetSelectedGameObject(GameObject.Find("ContinueButton"));                               //set default button
                            GameObject.Find("ContinueButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                        } else
                        {
                            eventSystem.SetSelectedGameObject(GameObject.Find("MainMenuButton"));                               //set default button
                            GameObject.Find("ContinueButton").SetActive(false);
                            GameObject.Find("MainMenuButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
                            GameObject.Find("MainMenuButton").GetComponent<RectTransform>().anchorMin = new Vector2(0.3f,0);
                            GameObject.Find("MainMenuButton").GetComponent<RectTransform>().anchorMax = new Vector2(0.7f, 1);



                        }
                        if (levelNum >= settingsList.Count)
                        {
                            GameObject.Find("ContinueButton").GetComponent<Button>().interactable = false;      //if on last level, grey out continueButton
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
                dropTimer = 0.0f;
                break;
            case (GameState.paused):
                GameObject[] turretList = GameObject.FindGameObjectsWithTag("Turret");
                for (int i = 0; i < turretList.Length; ++i)
                {
                    turretList[i].GetComponent<Turret>().inputDisabled = true;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    resumeButton();
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

        if (health * 1f / maxHealth >= lowHealthThreshold)
        {
            lowHealthText.enabled = false;
            shipDangerAudio.enabled = false;
        }
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
            if (!lowHealthText.enabled)
            {
                lowHealthText.enabled = true;
                shipDangerAudio.enabled = true;
                HelperFunctions.playSound(ref shipDangerAudio, shipDanger);
            }
        }
        else if (health * 1f / maxHealth >= lowHealthThreshold)
        {
            HelperFunctions.playSound(ref shipAudio, shipHit);
        }
        if(health <= 0)
        {
            playerIsDead = true;
            lowHealthText.enabled = false;
            shipDangerAudio.enabled = false;
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
            bombUsed = true;
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
    public void runPreGame()
    {
        if (!tutorialRunning)
        {
            StartCoroutine(levelTutorial(levelNum));
        }
    }
    public IEnumerator levelTutorial(int level)
    {
        
        tutorialRunning = true;
        settingsList[level].SetActive(false);
        for(int i = 0; i < currentSettings.targets.Length; ++i)
        {
            currentSettings.targets[i].SetActive(false);
        }
        for (int i = 0; i < currentSettings.tutorialUI.Length; ++i)
        {
            currentSettings.tutorialUI[i].SetActive(false);
        }
        if (currentSettings.tutorialUI.Length > 0)
        {
            currentSettings.tutorialUI[0].SetActive(true);
            outline.setPosition(currentSettings.tutorialUI[0].GetComponent<RectTransform>());
        }
        try
        {
            GameObject.FindGameObjectsWithTag("Turret")[0].GetComponent<Turret>().inputDisabled = true;
        }
        catch { }
        try
        {
            GameObject.FindGameObjectsWithTag("Turret")[1].GetComponent<Turret>().inputDisabled = true;
        }
        catch { }

        yield return new WaitForSeconds(preGameDelay);

        try
        {
            GameObject.FindGameObjectsWithTag("Turret")[0].GetComponent<Turret>().inputDisabled = false;
        }
        catch { }
        try
        {
            GameObject.FindGameObjectsWithTag("Turret")[1].GetComponent<Turret>().inputDisabled = false;
        }
        catch { }




        if (!survival)
        {
            //This switch is for levels that have tutorials
            switch (level)              //This is hard-coded into oblivion, I'm sorry
            {
                case (0):

                    for (int i = 0; i < currentSettings.tutorialUI.Length - 1; ++i)       //the last one (target tutorial) is a special case
                    {
                        currentSettings.tutorialUI[i].SetActive(true);
                        outline.setTarget(currentSettings.tutorialUI[i].GetComponent<RectTransform>()); //UI *MUST* be in precise order  
                        if (i != 0)
                            yield return new WaitForSeconds((float)(1 / 1.8));  //wait for turret cooldown
                        while (true)                                    //wait for player input
                        {
                            if (Input.GetKeyDown(KeyCode.UpArrow) && gameState != GameState.paused)      //advance after shot
                                break;
                            yield return null;
                        }
                        currentSettings.tutorialUI[i].SetActive(false);

                    }
                    currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].SetActive(true);
                    outline.setTarget(currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].GetComponent<RectTransform>());
                    currentSettings.targets[0].SetActive(true);
                    while (!currentSettings.targets[0].GetComponent<Target>().activated)
                    {
                        yield return null;
                    }    //wait for player input
                    currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].SetActive(false);
                    outline.gameObject.SetActive(false);
                    break;
                case (1):
                    for (int i = 0; i < currentSettings.tutorialUI.Length - 2; ++i)       //the last one (target tutorial) is a special case
                    {
                        if (i == 1)
                            currentSettings.tutorialUI[2].SetActive(true);
                        currentSettings.tutorialUI[i].SetActive(true);
                        outline.setTarget(currentSettings.tutorialUI[i].GetComponent<RectTransform>()); //UI *MUST* be in precise order  
                        if (i != 0)
                            yield return new WaitForSeconds((float)(1 / 1.8));  //wait for turret cooldown
                        while (true)                                    //wait for player input
                        {
                            if (Input.GetKeyDown(KeyCode.W) && gameState != GameState.paused)      //advance after shot
                                break;
                            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && gameState != GameState.paused)
                            {
                                dropBomb();
                                break;
                            }
                            if (bombUsed)
                            {
                                bombCooldown -= Time.deltaTime;

                                if (bombCooldown <= 0)
                                {
                                    bombUsed = false;
                                    bombCooldown = setBombCooldown;
                                }
                            }
                            yield return null;
                        }
                        if (i == 1)
                            currentSettings.tutorialUI[2].SetActive(false);
                        currentSettings.tutorialUI[i].SetActive(false);


                    }
                    currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].SetActive(true);
                    outline.setTarget(currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].GetComponent<RectTransform>());
                    currentSettings.targets[0].SetActive(true);
                    while (!currentSettings.targets[0].GetComponent<Target>().activated)
                    {
                        yield return null;
                    }    //wait for player input
                    currentSettings.tutorialUI[currentSettings.tutorialUI.Length - 1].SetActive(false);
                    outline.gameObject.SetActive(false);
                    bombs = 3;                                      //reset bombs if the player used any
                    break;
                case (2):
                    currentSettings.tutorialUI[0].SetActive(true);
                    while (true)                                    //wait for player input
                    {
                        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && gameState != GameState.paused)      //advance after shot
                            break;
                        yield return null;
                    }
                    currentSettings.tutorialUI[0].SetActive(false);
                    currentSettings.tutorialUI[1].SetActive(true);
                    currentSettings.tutorialUI[2].SetActive(true);
                    outline.setTarget(currentSettings.tutorialUI[1].GetComponent<RectTransform>());
                    GameObject.Find("DynamicOutline2").GetComponent<UIOutline>().setPosition(currentSettings.tutorialUI[0].GetComponent<RectTransform>());
                    GameObject.Find("DynamicOutline2").GetComponent<UIOutline>().setTarget(currentSettings.tutorialUI[2].GetComponent<RectTransform>());



                    for (int i = 0; i < currentSettings.targets.Length; ++i)
                    {
                        currentSettings.targets[i].SetActive(true);
                    }
                    while (true)
                    {
                        bool startGame = true;
                        for (int i = 0; i < currentSettings.targets.Length; ++i)
                        {
                            if (!currentSettings.targets[i].GetComponent<Target>().activated)
                                startGame = false;
                        }
                        if (startGame)
                            break;
                        yield return null;
                    }
                    currentSettings.tutorialUI[1].SetActive(false);
                    currentSettings.tutorialUI[2].SetActive(false);
                    GameObject.Find("DynamicOutline2").GetComponent<UIOutline>().setPosition(
                        new Vector3(1000, 1000, 0), new Vector3(1000, 1000, 0));

                    break;
                default:
                    for (int i = 0; i < currentSettings.targets.Length; ++i)
                    {
                        currentSettings.targets[i].SetActive(true);
                    }
                    while (true)
                    {
                        bool startGame = true;
                        for (int i = 0; i < currentSettings.targets.Length; ++i)
                        {
                            if (!currentSettings.targets[i].GetComponent<Target>().activated)
                                startGame = false;
                        }
                        if (startGame)
                            break;
                        yield return null;
                    }

                    break;
            }
        }
        tutorialRunning = false;
        startLevel();
        yield return null;
    }
    public void startLevel()
    {
        gameState = GameState.duringGame;
        settingsList[levelNum].SetActive(true);
        for (int i = 0; i < currentSettings.targets.Length; ++i)
        {
            
            currentSettings.targets[i].GetComponent<Target>().startColorChange();
            currentSettings.targets[i].GetComponent<DelAfterTime>().startTimer();
            currentSettings.targets[i].GetComponent<PositionOscillation>().stopOscillation();
            currentSettings.targets[i].GetComponent<ConstantRotation>().stopRotation();
        }
        for (int i = 0; i < currentSettings.enabledUI.Length; i++)
        {
            try
            {
                currentSettings.tutorialUI[i].SetActive(false);
            }
            catch { };  //this will trigger if an object was deleted, but that's fine
        }
        if(outline != null)
            outline.setPosition(new Vector3(1000, 1000, 0), new Vector3(1000, 1000, 0));

    }
    public IEnumerator cameraShake (float duration, float magnitude)
    {
        Vector3 originalPos = cam.position;

        for (float elapsed = 0.0f; elapsed < duration; elapsed += Time.deltaTime)
        {
            float x = Random.Range(-magnitude, magnitude) * (1 - elapsed / duration);
            float y = Random.Range(-magnitude, magnitude) * (1 - elapsed / duration);
            cam.position = originalPos + new Vector3(x, y, 0);
            yield return new WaitForSeconds(Time.deltaTime);
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
        LevelNumber.setLoadedFromMenu(true);
        eventSystem.SetSelectedGameObject(null);
        StartCoroutine(transitionIntoSceneLoad(0.5f, "MainMenu"));
        if (survival)
            setHighScore(points);
    }
    public void continueButton()
    {
        StartCoroutine(loadNextLevel());
        eventSystem.SetSelectedGameObject(null);
        levelCompletionCanvas.SetActive(false);
        
    }
    public void backButton()
    {
        gameState = GameState.survivalDeathState;
        deathCanvas.SetActive(true);
        highScoreCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(GameObject.Find("RestartButton"));                               //set default button
        GameObject.Find("RestartButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
    }
    public void highScoreButton()
    {
        gameState = GameState.highScore;
        highScoreCanvas.SetActive(true);
        deathCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(GameObject.Find("BackButton"));                               //set default button
        GameObject.Find("BackButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));  //force highlight button
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
        Destroy(GameObject.FindGameObjectWithTag("StarList"));
        SceneManager.LoadScene("Campaign");
    }
    public void restartSurvival()
    {
        SceneManager.LoadScene("Survival");
    }
    public void resumeButton()
    {
        GameObject[] turretList = GameObject.FindGameObjectsWithTag("Turret");

        if (!tutorialRunning)
        {
            gameState = GameState.duringGame;
        }
        else
        {
            gameState = GameState.beforeGame;
        }
        pauseCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        Time.timeScale = 1;
        for (int i = 0; i < turretList.Length; ++i)
        {
            turretList[i].GetComponent<Turret>().inputDisabled = false;
        }
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
            result[i] = temp;
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
        highScoreCanvas.GetComponentInChildren<HighScoreList>().updateHighScores(highScores);
    }

    public void sortHighScores(ref List<HighScore> hs)
    {
        hs.Sort(delegate (HighScore a, HighScore b) { return -(a.score - b.score); });
    }

    public void changeMultiplier(int hits)
    {
        if (hits == 0)
        {
            int temp = (int)(multiplier * 2 * missMultiplier);
            multiplier = (float)temp / 2;
            multiplier = (multiplier < 1 ? 1 : multiplier);
        }
        else if (hits > 1)
            multiplier += (hits - 1) * 0.5f;
    }

    public bool willDrop()
    {
        return dropTimer >= timeToDrop;
    }

    public IEnumerator advanceDropTimer()
    {
        if (dropTimer == 0.0f)
        {
            timeToDrop = Random.Range(dropTimes.x, dropTimes.y);
            while (dropTimer < timeToDrop)
            {
                yield return new WaitForEndOfFrame();
                dropTimer += Time.deltaTime;
            }
        }
    }
    public IEnumerator drainFTLBar(float rate)
    {
        float frac = 1f;
        while(chargeBar.percent >= 0)
        {
            if (gameState == GameState.beforeGame)
            {
                frac -= rate * Time.deltaTime;
                chargeBar.setProgress(frac);
            } else
            {
                chargeBar.setProgress(0f);
                break;
            }
            yield return null;
        }
    }
    public IEnumerator transitionIntoSceneLoad(float transitionTime, string sceneName)
    {
        blackPanel.initialDelay = 0;
        blackPanel.duration = transitionTime;
        blackPanel.setColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1));
        blackPanel.startColorChange();
        yield return new WaitForSeconds(blackPanel.duration);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator playSongs ()
    {
        while (true)
        {
            AudioClip song = musicList[Random.Range(0, musicList.Count)];
            HelperFunctions.playSound(ref musicSource, song);
            yield return new WaitForSeconds(song.length);
        }
    }
}

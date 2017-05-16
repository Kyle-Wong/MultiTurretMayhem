using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour {

	// Use this for initialization
    public enum MenuState
    {
        intro,
        main,
        levelSelect,
        credits
    }
    private bool firstPass = true;
    private MenuState menuState;
    public GameObject introCanvas;
    public GameObject mainCanvas;
    public GameObject levelSelectCanvas;
    public GameObject creditsCanvas;
    private GraphicColorLerp blackPanel;
    private LevelSelectController levelSelectController;
    public EventSystem eventSystem;
    public AudioClip menuMusic;
    public AudioClip soundOnMove;
    public AudioClip soundOnConfirm;
    public AudioSource menuEffects;
    public AudioSource musicSource;
    public bool skipIntro = false;
    public bool hideMouse = true;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    void Start () {
        musicSource.clip = menuMusic;
        skipIntro = LevelNumber.getSkipIntro();
        blackPanel = GameObject.Find("BlackPanel1").GetComponent<GraphicColorLerp>();
        levelSelectController = levelSelectCanvas.GetComponent<LevelSelectController>();
        mainCanvas.SetActive(false);
        levelSelectCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        if (skipIntro)
        {
            setMenuState((int)MenuState.main);
            GameObject.Find("StarList").GetComponent<FTLJumpWithImage>().stopAllStars();
            LevelNumber.setSkipIntro(false);
        } else
        {
            setMenuState((int)MenuState.intro);

        }
        if (hideMouse)
        {
            Cursor.visible = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (menuState)
        {
            case (MenuState.intro):
                break;
            case (MenuState.main):
                break;
            case (MenuState.levelSelect):
                break;
            case (MenuState.credits):
                break;
        }
	}
    public void setMenuState(int x)
    {
        menuState = (MenuState)x;
        switch (menuState)
        {
            case (MenuState.intro):
                if(introCanvas.activeSelf == false)
                    introCanvas.SetActive(true);
                introCanvas.GetComponent<GameIntroSequence>().playIntro();
                break;
            case (MenuState.main):
                if (firstPass)
                {
                    musicSource.volume = .2f;
                    HelperFunctions.playSound(ref musicSource, menuMusic);
                    firstPass = false;
                }
                creditsCanvas.SetActive(false);
                levelSelectCanvas.SetActive(false);
                introCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                eventSystem.SetSelectedGameObject(GameObject.Find("Campaign"));
                GameObject.Find("Campaign").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));
                break;
            case (MenuState.levelSelect):
                mainCanvas.SetActive(false);
                levelSelectCanvas.SetActive(true);
                GameObject.Find("1").GetComponent<UIScroller>().revealUI(1);
                levelSelectController.levelIndex = 0;
                eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
                GameObject.Find("PlayButton").GetComponent<Button>().OnSelect(new BaseEventData(EventSystem.current));
                break;
            case (MenuState.credits):
                break;
        }
    }
    public void campaignPress()
    {
        confirmSound();
        setMenuState((int)MenuState.levelSelect);
    }
    public void survivalPress()
    {
        confirmSound();
        StartCoroutine(loadSurvival());
        eventSystem.SetSelectedGameObject(null);
    }
    private IEnumerator loadSurvival()
    {
        blackPanel.gameObject.GetComponent<Image>().enabled = true;
        blackPanel.startColorChange();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Survival");
    }
    public void creditsPress()
    {
        confirmSound();
        menuState = MenuState.levelSelect;
    }
    public void quitPress()
    {
        Application.Quit();
    }
    public void navigateSound()
    {
        menuEffects.clip = soundOnMove;
        menuEffects.PlayDelayed(.1f);
    }
    public void confirmSound()
    {
        menuEffects.clip = soundOnConfirm;
        menuEffects.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameIntroSequence : MonoBehaviour {

    // Use this for initialization
    private MenuController menuController;
    public bool playOnStartUp = true;
    public UIMovement ship;
    public UIMovement title;
    public UIMovement prompt;
    public Image boost;
    public FTLJumpWithImage FTLJump;
    public float FTLDuration;
    public float delayUntilInput;
    public bool allowInput = false;
    public AudioClip introMusic;
    public AudioClip boosterSound;
    public AudioClip confirmSound;
    public AudioSource musicSource;

    void Start () {
        menuController = GameObject.Find("MainMenuController").GetComponent<MenuController>();
        if(playOnStartUp)
            StartCoroutine(intro());
	}
	
	// Update is called once per frame
	void Update () {
        if (allowInput)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2)) 
                { } //can't use mouse clicks to get in
                else
                    StartCoroutine(outro());
            }
        }
	}
    public void playIntro()
    {
        StartCoroutine(intro());
    }
    private IEnumerator intro()
    {
        FTLJump.startAllStars();
        HelperFunctions.playSound(ref musicSource, boosterSound);
        StartCoroutine(HelperFunctions.interpolateSound(musicSource, 2));
        yield return new WaitForSeconds(FTLDuration);
        FTLJump.stopAllStars();
        ship.startMovement();
        title.startMovement();
        prompt.startMovement();
        boost.enabled = false;
        StartCoroutine(HelperFunctions.negInterpolateSound(musicSource, 1.25f));
        yield return new WaitForSeconds(delayUntilInput);
        HelperFunctions.playSound(ref musicSource, introMusic);
        StartCoroutine(HelperFunctions.interpolateSound(musicSource, .5f));

        allowInput = true;                                      //once this intro is done, allow input
    }
    private IEnumerator outro()
    {
        allowInput = false;
        GetComponent<AudioSource>().clip = confirmSound;
        GetComponent<AudioSource>().Play();
        for (byte color = 255; color > 0; color -= 5)
        {
            prompt.gameObject.gameObject.GetComponent<Text>().color = new Color32(0, 255, 255, color);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(.3f);
        menuController.firstPass = false;
        menuController.setMenuState(1);     //go to main state
    }
}

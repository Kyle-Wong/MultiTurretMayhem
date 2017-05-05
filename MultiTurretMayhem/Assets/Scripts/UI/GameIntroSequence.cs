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
                menuController.setMenuState(1);     //go to main state
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
        HelperFunctions.playSound(ref musicSource, introMusic);
        StartCoroutine(HelperFunctions.interpolateSound(musicSource, 2));
        yield return new WaitForSeconds(FTLDuration);
        FTLJump.stopAllStars();
        ship.startMovement();
        title.startMovement();
        prompt.startMovement();
        boost.enabled = false;
        yield return new WaitForSeconds(delayUntilInput);
        allowInput = true;                                      //once this intro is done, allow input
    }
}

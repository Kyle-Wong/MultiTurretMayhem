using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	// Use this for initialization
    public enum MenuState
    {
        main,
        levelSelect,
        credits
    }
    private MenuState menuState;
	void Start () {
        menuState = MenuState.main;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void campaignPress()
    {
        menuState = MenuState.levelSelect;
    }
    public void survivalPress()
    {
        SceneManager.LoadScene("Survival");
    }
    public void creditsPress()
    {
        menuState = MenuState.levelSelect;
    }
    public void quitPress()
    {
        Application.Quit();
    }
}

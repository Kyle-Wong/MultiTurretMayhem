using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LevelSelectController : MonoBehaviour {

    // Use this for initialization
    public GameObject levelNumParent;
    private List<UIScroller> levels;
    private MenuController menuController;

    public int levelIndex;
	void Start () {
        menuController = GameObject.Find("MainMenuController").GetComponent<MenuController>();
        
        levels = getScollers(levelNumParent);
        levelIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}
    private List<UIScroller> getScollers(GameObject obj)
    {
        List<UIScroller> children = new List<UIScroller>();         //get all children of this gameobject
                                                                    
        foreach (Transform child in obj.transform)
        {
            children.Add(child.gameObject.GetComponent<UIScroller>());
        }
        return children;
    }
    public void scrollLeft()
    {
        if(levelIndex > 0)
        {
            levels[levelIndex--].hideUI(-1);
            levels[levelIndex].revealUI(-1);
        } 
    }
    public void scrollRight()
    {
        if(levelIndex < levels.Count-1)
        {
            levels[levelIndex++].hideUI(1);
            levels[levelIndex].revealUI(1);
        } 
    }
    public void playPress()
    {
        //need a way to send level number through scenes 
        LevelNumber.setLevel(levelIndex);
        SceneManager.LoadScene("Campaign");
    }
    public void backPress()
    {
        levels[levelIndex].hideImmediately();
        menuController.setMenuState(1);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreList : MonoBehaviour {

    // Use this for initialization
    List<HighScore> highScoreList;
    public Text[] textList;
    private gameController controller;
	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void updateHighScores(List<HighScore> list)
    {
        highScoreList = list;
        for(int i = 0; i < textList.Length; ++i)
        {
            print(list[i].scoreAsString());
            textList[i].text =list[i].scoreAsString();
        }
    }
}

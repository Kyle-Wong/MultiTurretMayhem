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
        highScoreList = getHighScores();
	}
	
	// Update is called once per frame
	void Update () {
		
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
    public void updateHighScores()
    {
        highScoreList = getHighScores();
        for(int i = 0; i < textList.Length; ++i)
        {
            textList[i].text = highScoreList[i].scoreAsString(5);
        }
    }
}

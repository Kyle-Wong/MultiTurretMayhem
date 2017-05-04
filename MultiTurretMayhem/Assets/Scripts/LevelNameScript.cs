using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelNameScript : MonoBehaviour {

    // Use this for initialization
    private Text text;
	void Start () {
        text = GetComponent<Text>();
        text.text = "Level " + (LevelNumber.getLevel()+1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

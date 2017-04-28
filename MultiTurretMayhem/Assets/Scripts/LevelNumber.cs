using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumber : MonoBehaviour {

    // Use this for initialization
    static int level = 0;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static int getLevel()
    {
        return level;
    }
    public static void setLevel(int x)
    {
        level = x;
    }
}

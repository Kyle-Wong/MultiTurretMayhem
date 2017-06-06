using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindUISoundObject : MonoBehaviour {

    // Use this for initialization
    private UISoundController ctrl;     //this is a terrible class to write

	void Start () {
        ctrl = GameObject.FindGameObjectWithTag("UISoundObject").GetComponent<UISoundController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void playConfirmSound()
    {
        ctrl.playConfirmSound();
    }
    public void playSelectSound()
    {
        ctrl.playSelectSound();
    }
}

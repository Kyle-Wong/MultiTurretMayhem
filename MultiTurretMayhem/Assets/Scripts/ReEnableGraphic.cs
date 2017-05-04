using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReEnableGraphic : MonoBehaviour {

    // Use this for initialization
    private Graphic graphicRenderer;
	void Start () {
        graphicRenderer = GetComponent<Graphic>();
        graphicRenderer.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

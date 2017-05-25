using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelNameScript : MonoBehaviour {

    // Use this for initialization
    private Text text;
    private GraphicColorLerp colorLerp;
    private bool LoadedFromMenu;
    void Awake()
    {
        LoadedFromMenu = LevelNumber.getLoadedFromMenu();
    }
	void Start () {
        text = GetComponent<Text>();
        text.text = "Level " + (LevelNumber.getLevel()+1);
        colorLerp = GetComponent<GraphicColorLerp>();
        if (!LoadedFromMenu)
        {
            StartCoroutine(displayText(1.5f));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public IEnumerator displayText(float holdColorDuration)
    {
        colorLerp.initialDelay = 0;
        Color startColor = text.color;
        Color endColor = startColor;
        startColor.a = 0;
        colorLerp.setColors(startColor, endColor);
        colorLerp.startColorChange();
        yield return new WaitForSeconds(colorLerp.duration + holdColorDuration);
        colorLerp.setColors(endColor, startColor);
        colorLerp.startColorChange();
    }
}

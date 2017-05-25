using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour {

    // Use this for initialization
    public float percent;
    private RectTransform rect;
    public float colorChangeThreshold;
    public Color newColor;
    public Color fullColor;
    private Image myImage;
	void Start () {
        rect = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(percent >= 1f)
        {
            myImage.color = fullColor;
        }
		else if(percent >= colorChangeThreshold)
        {
            myImage.color = newColor;
        }
	}
    public void setProgress(float frac)
    {
        if(rect == null)
            rect = GetComponent<RectTransform>();
        percent = frac;
        rect.anchorMax = new Vector2(frac, 1);
    }
}

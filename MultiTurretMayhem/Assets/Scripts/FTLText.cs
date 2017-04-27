using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTLText : MonoBehaviour {

    // Use this for initialization
    public float percent;
    private Text text;
    public float textChangeThreshold;
    public string normalText;
    public string thresholdText;
    public string fullText;
    private Image myImage;
    void Start () {
        text = GetComponent<Text>();
        text.text = normalText;
	}
	
	// Update is called once per frame
	void Update () {
       
        if (percent >= 1)
        {
            text.text = fullText;
        }
        else if (percent >= textChangeThreshold)
        {
            text.text = thresholdText;
        }

		
	}
    public void setPercent(float frac)
    {
        percent = frac;
    }
    

}

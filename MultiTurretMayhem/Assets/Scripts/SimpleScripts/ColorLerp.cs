using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour {

    // Use this for initialization
    public bool playOnStartUp = true;
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public float duration = 0;
    public float initialDelay = 0;
    private SpriteRenderer spriteRenderer;
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (playOnStartUp)
        {
            startColorChange();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator colorChange()
    {
        while(initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
            yield return null;
        }
        float elapsedTime = 0;
        if(duration == 0)
        {
            spriteRenderer.color = endColor;
        }
        while(elapsedTime < duration)
        {
            spriteRenderer.color = Color.Lerp(startColor,
                endColor, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void startColorChange()
    {
        StartCoroutine(colorChange());
    }
    public void setColors(Color newStartColor, Color newEndColor)
    {
        startColor = newStartColor;
        endColor = newEndColor;
    }
}

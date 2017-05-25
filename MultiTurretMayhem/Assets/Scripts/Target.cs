using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    // Use this for initialization
    public bool activated;
    public float activeDuration;
    private float timer;
    private SpriteRenderer spriteRenderer;
    private ColorLerp colorLerp;
	void Start () {
        timer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorLerp = GetComponent<ColorLerp>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            spriteRenderer.color = Color.green;
        }else
        {
            spriteRenderer.color = Color.red;
            activated = false;
        }
	}

    public void activate()
    {
        activated = true;
        timer = activeDuration;
    }
    public void startColorChange()
    {
        colorLerp.setColors(new Color(0, 1, 0, 1), new Color(0, 1, 0, 0));
        colorLerp.duration = 1f;
        colorLerp.startColorChange();
    }
}

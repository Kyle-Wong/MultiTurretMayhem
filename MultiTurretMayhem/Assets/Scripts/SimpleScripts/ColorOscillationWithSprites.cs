using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorOscillationWithSprites : MonoBehaviour
{

    // Use this for initialization
    public bool playOnStartUp = true;
    public SpriteRenderer spriteRenderer;
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    private const int POSITIVE = 1;
    private const int NEGATIVE = -1;
    private int direction;
    public float transitionTime;
    public float timer;
    public bool stopped;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stopped = true;
        if (playOnStartUp)
            startColorChange();
        direction = POSITIVE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            if (timer < 0)
                direction = POSITIVE;
            if (timer > transitionTime)
                direction = NEGATIVE;
            timer += Time.deltaTime * direction;

            spriteRenderer.color = Color.Lerp(startColor, endColor, timer / transitionTime);
        }
    }
    public void startColorChange()
    {
        stopped = false;
    }
    public void stopColorChange()
    {
        stopped = true;
    }
}

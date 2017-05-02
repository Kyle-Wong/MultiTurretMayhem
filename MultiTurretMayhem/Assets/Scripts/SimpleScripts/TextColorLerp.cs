using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorLerp : MonoBehaviour
{

    // Use this for initialization
    public bool playOnStartUp = true;
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public float duration = 0;
    public float initialDelay = 0;
    private Text text;
    private IEnumerator colorCoroutine;
    void Start()
    {
        text = GetComponent<Text>();
        colorCoroutine = colorChange();
        if (playOnStartUp)
        {
            startColorChange();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator colorChange()
    {
        text.color = startColor;
        while (initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
            yield return null;
        }
        float elapsedTime = 0;
        if (duration == 0)
        {
            text.color = endColor;
        }
        while (elapsedTime < duration)
        {
            text.color = Color.Lerp(startColor,
                endColor, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        text.color = endColor;
    }
    public void startColorChange()
    {
        StopCoroutine(colorCoroutine);
        colorCoroutine = colorChange();
        StartCoroutine(colorCoroutine);
    }
    public void setColors(Color newStartColor, Color newEndColor)
    {
        startColor = newStartColor;
        endColor = newEndColor;
    }
}

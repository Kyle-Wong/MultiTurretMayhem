using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreditsScript : MonoBehaviour {

    // Use this for initialization
    public UIRevealer[] names;
    public UIRevealer[] roles;
    public float initialDelay;
    public float delay;
    private IEnumerator coroutine;
	void Start () {
        coroutine = creditsSequence();
	}
	
	// Update is called once per frame

    public void startCredits()
    {
        coroutine = creditsSequence();
        StartCoroutine(coroutine);
    }
    public void stopCredits()
    {
        StopCoroutine(coroutine);
        for (int i = 0; i < names.Length; ++i)
        {
            names[i].hideImmediately();
            roles[i].hideImmediately();
        }
    }
    public IEnumerator creditsSequence()
    {
        yield return new WaitForSeconds(initialDelay);
        for(int i = 0; i < names.Length; ++i)
        {
            names[i].revealUI();
            roles[i].revealUI();
            yield return new WaitForSeconds(delay);
        }
    }
}

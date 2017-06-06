using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour {

    // Use this for initialization
    public AudioClip selectSound;
    public AudioClip confirmSound;
    private AudioSource source;
	void Awake () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void playSelectSound()
    {
        source.clip = selectSound;
        source.Play();
    }
    public void playConfirmSound()
    {
        source.clip = confirmSound;
        source.Play();
    }

}

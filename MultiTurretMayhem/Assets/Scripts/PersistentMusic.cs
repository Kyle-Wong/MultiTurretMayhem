using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMusic : MonoBehaviour {
    private AudioSource musicSource;
    public List<AudioClip> musicList;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(playSongs());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private IEnumerator playSongs()
    {
        while (true)
        {
            AudioClip song = musicList[Random.Range(0, musicList.Count)];
            HelperFunctions.playSound(ref musicSource, song);
            yield return new WaitForSeconds(song.length);
        }
    }
}

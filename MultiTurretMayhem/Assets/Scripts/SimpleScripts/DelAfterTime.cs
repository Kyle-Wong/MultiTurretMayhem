using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelAfterTime : MonoBehaviour {

    // Use this for initialization
    public bool playOnStartUp = true;
    public float lifeTime = 0;
	void Start () {
        if (playOnStartUp)
        {
            startTimer();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator destroyAfterTime()
    {
        while(lifeTime >= 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);   
    }
    public void startTimer()
    {
        StartCoroutine(destroyAfterTime());
    }
}

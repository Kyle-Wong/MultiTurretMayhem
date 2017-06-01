using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOscillation : MonoBehaviour {

    // Use this for initialization
    public enum PlayOn
    {
        Awake,
        Enable,
        None
    }
    public PlayOn playOn;
    public float magnitude;
    public float period;
    private Vector3 myPos;
    private float timer;
    private bool active;
	void Awake () {
        myPos = transform.position;

        timer = 0;
        if(playOn == PlayOn.Awake)
        {
            active = true;
        }
	}
    void OnEnable()
    {
        if(playOn == PlayOn.Enable)
        {
            active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (timer > period)
            {
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
            transform.position = myPos + Vector3.up * magnitude * Mathf.Sin(6.28f * (timer / period));
        }
    }
    public void startOscillation()
    {
        active = true;
    }
    public void stopOscillation()
    {
        active = false;
    }
}

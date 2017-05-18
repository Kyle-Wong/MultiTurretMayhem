using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public int healthAdd;
    public int bombAdd;
    public Color effectColor;
    private const int MAX_HEALTH = 8;
    private const int MAX_BOMBS = 5;
    private gameController ctrl;
    private float pulseTime = 2;
    private float pulseTimer = 0.0f;

    void Awake()
    {
        ctrl = GameObject.Find("GameController").GetComponent<gameController>();
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(advancePulserTimer());
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (pulseTimer >= pulseTime)
        {
            pulseTimer = 0;
            Pulse();
            StartCoroutine(advancePulserTimer());
        }
	}

    public void Pulse()
    {
        GameObject pulse = (GameObject)Instantiate(Resources.Load("PulseBomb"), transform.position, Quaternion.identity);
        ColorLerp bombColor = pulse.transform.GetChild(0).gameObject.GetComponent<ColorLerp>();
        bombColor.startColor = effectColor;
        bombColor.endColor = new Color(effectColor.r, effectColor.g, effectColor.b, 0);
    }

    public void apply()
    {
        GameObject effectBomb = (GameObject)Instantiate(Resources.Load("PickupBomb"), transform.position, Quaternion.identity);
        ColorLerp bombColor = effectBomb.transform.GetChild(0).gameObject.GetComponent<ColorLerp>();
        bombColor.startColor = effectColor;
        bombColor.endColor = new Color(effectColor.r, effectColor.g, effectColor.b, 0);

        if(ctrl.bombs < MAX_BOMBS)
            ctrl.bombs += bombAdd;
        if(ctrl.getHealth() < MAX_HEALTH)
            ctrl.setHealth(ctrl.getHealth() + healthAdd);
        Destroy(gameObject);
    }

    private IEnumerator advancePulserTimer()
    {
        while (pulseTimer < pulseTime)
        {
            yield return new WaitForEndOfFrame();
            pulseTimer += Time.deltaTime;
        }
    }
}

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
    private float pulseTime = 3.3333f;
    private float pulseTimer;
    private AudioSource _audioSource;
    public AudioClip pickUpSound;
    public bool pickedUp = false;

    void Awake()
    {
        ctrl = GameObject.Find("GameController").GetComponent<gameController>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        pulseTimer = pulseTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (pulseTimer >= pulseTime && !pickedUp)
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

        if (ctrl.bombs < MAX_BOMBS)
            ctrl.bombs += bombAdd;
        if (ctrl.getHealth() < MAX_HEALTH)
            ctrl.setHealth(ctrl.getHealth() + healthAdd);
        HelperFunctions.playSound(ref _audioSource, pickUpSound);
        foreach (GameObject g in HelperFunctions.getChildren(gameObject.transform)) {
            if (g.GetComponent<SpriteRenderer>() != null)
                g.GetComponent<SpriteRenderer>().color = Color.clear;
            
        }
        pickedUp = true;
        Destroy(gameObject, 0);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFieldGenerator : MonoBehaviour {

    // Use this for initialization
    public int starNum;
    public float starDistance;
    private List<Star> starList;
    public GameObject parent;
    private SpriteRenderer boostSprite;
	void Start () {
        starList = new List<Star>();
        spawnStars(starNum, starDistance);
        boostSprite = GameObject.Find("BOOST").GetComponent<SpriteRenderer>();
        boostSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            startAllStars();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            stopAllStars();
        }
        if(starList.Count > 0)
        {
            if(starList[0].GetComponent<Star>().getSpeed() > 0)
            {
                parent.transform.DetachChildren();
                parent.GetComponent<ConstantRotation>().stopRotation();
            }
            else
            {
                for (int i = 0; i < starList.Count; i++)
                {
                    starList[i].gameObject.transform.parent = parent.transform;
                }
                parent.GetComponent<ConstantRotation>().startRotation();
            }
        }
	}
    public void spawnStars(int num, float distance)
    {
        for(int i = 0; i < num; i++)
        {
            float rngX = Random.Range(-distance, distance);
            float rngY = Random.Range(-distance, distance);
            GameObject star = (GameObject)Instantiate(Resources.Load("Star1"));
            star.transform.position = new Vector3(rngX, rngY);
            star.transform.parent = parent.transform;
            starList.Add(star.GetComponent<Star>());
            star.GetComponent<Star>().setBounds(distance);
        }
    }
    public void startAllStars()
    {
        for(int i = 0; i < starList.Count; i++)
        {
            starList[i].startMoving();
        }
        boostSprite.enabled = true;

    }
    public void stopAllStars()
    {
        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].stopMoving();
        }
        boostSprite.enabled = false;

    }
}

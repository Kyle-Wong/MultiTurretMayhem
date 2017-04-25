using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTLJump : MonoBehaviour {

    // Use this for initialization
    private List<Star> starList;
    public SpriteRenderer boostSprite;
    private ConstantRotation myRotator;
    void Start () {
        boostSprite.enabled = false;
        myRotator = GetComponent<ConstantRotation>();
        starList = getStarList();
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
        if (starList.Count > 0)
        {
            if (starList[0].GetComponent<Star>().getSpeed() > 0)
            {
                transform.DetachChildren();
                myRotator.stopRotation();
            }
            else
            {
                for (int i = 0; i < starList.Count; i++)
                {
                    starList[i].gameObject.transform.parent = transform;
                }
                myRotator.startRotation();
            }
        }
    }
    private List<Star> getStarList()
    {
        List<Star> children = new List<Star>();
        foreach (Transform child in transform)
        {
           children.Add(child.gameObject.GetComponent<Star>());
        }
        return children;
    }
    public void startAllStars()
    {
        for (int i = 0; i < starList.Count; i++)
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

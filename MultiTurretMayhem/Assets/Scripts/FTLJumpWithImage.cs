using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FTLJumpWithImage : MonoBehaviour {

    // Use this for initialization
    private List<Star> starList;
    public Image boostImage;
    private ConstantRotation myRotator;
    public AudioSource ftlSource;
    public AudioClip ftlSound;
    void Awake()
    {
        boostImage.enabled = false;
        myRotator = GetComponent<ConstantRotation>();
        starList = getStarList();
    }

    // Update is called once per frame
    void Update()
    {
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
        HelperFunctions.playSound(ref ftlSource, ftlSound);
        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].startMoving();
        }
        boostImage.enabled = true;

    }
    public void stopAllStars()
    {
        ftlSource.Stop();
        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].stopMoving();
        }
        boostImage.enabled = false;

    }
}

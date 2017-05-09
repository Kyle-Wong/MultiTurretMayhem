using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Splitter : Enemy
{

    public GameObject splitterChild;
    public int splittersToSpawn = 3;
    private float deltaDistance = 5;

    public AudioClip splitSound;
    // Use this for initialization
    void Start()
    {
        baseStart();
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        
    }
    public override void takeDamage(int damage, Color deathColor)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            for (int i = 0; i < splittersToSpawn; i++)
            {
                GameObject splitter = (GameObject)Instantiate(splitterChild, gameObject.transform.position, Quaternion.identity);
                splitter.GetComponent<Splitter_c>().ID = i;
            }
            HelperFunctions.playSound(ref _audioSource, splitSound);
            die(deathColor, false);
        }

    }
    public override void die(Color deathColor, bool byBomb)
    {
        if (!isDead)
        {
            if (!byBomb)
            {
                controller.addPoints((int)(points * ctrl.multiplier));
                GameObject p = Instantiate(pointsText, canvas.transform);
                p.GetComponent<RectTransform>().localPosition = HelperFunctions.objectCameraConvert(transform.position, canvas, cam);
                p.GetComponent<Text>().text = ((int)(points * ctrl.multiplier)).ToString();
            }

            HelperFunctions.playSound(ref _audioSource, deathSound);

            isDead = true;
            invincible = true;

            if (GetComponent<Simple_Movement>() != null)
            {
                GetComponent<Simple_Movement>().stopMovement();
            }
            GameObject spriteObject1 = transform.GetChild(0).gameObject;
            GameObject spriteObject2 = transform.GetChild(1).gameObject;
            if (spriteObject1.GetComponent<ConstantRotation>() != null)
            {
                spriteObject1.GetComponent<ConstantRotation>().setSpeed(0);

            }
            if (spriteObject2.GetComponent<ConstantRotation>() != null)
            {
                spriteObject2.GetComponent<ConstantRotation>().setSpeed(0);

            }



            ColorLerp colorLerp = spriteObject1.GetComponent<ColorLerp>();
            colorLerp.startColor = deathColor;
            colorLerp.endColor = new Color(deathColor.r, deathColor.g, deathColor.b, 0);
            colorLerp.startColorChange();

            colorLerp = spriteObject2.GetComponent<ColorLerp>();
            colorLerp.startColor = deathColor;
            colorLerp.endColor = new Color(deathColor.r, deathColor.g, deathColor.b, 0);
            colorLerp.startColorChange();
            Destroy(gameObject, colorLerp.initialDelay + colorLerp.duration);

        }

    }
    Vector3 generateRandomDistance()
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
        float newX = toPlayer.x + Random.Range(-1f, 1f) * deltaDistance;
        float newY = toPlayer.y + Random.Range(-1f, 1f) * deltaDistance;
        return new Vector3(newX, newY, 1f);
    }
    void launchAway(GameObject splitter)
    {
        Vector3 toPlayer = GetComponent<Simple_Movement>().getPlayerDir();
    }

}
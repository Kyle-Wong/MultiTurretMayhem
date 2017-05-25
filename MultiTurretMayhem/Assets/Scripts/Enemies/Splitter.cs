using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Splitter : Enemy
{

    public GameObject splitterChild;
    public int splittersToSpawn = 2;
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
                splitter.GetComponent<Splitter_c>().ID = i*2;
            }
            HelperFunctions.playSound(ref _audioSource, splitSound);
            die(Color.clear, false);
        }

    }

    public override void die(Color deathColor, bool byBomb)
    {
        if (!isDead)
        {
            if (!byBomb) // Die not by bomb
            {
                if (controller.survival) // Add points and drop item
                {
                    controller.addPoints((int)(points * ctrl.multiplier));
                    GameObject p = Instantiate(pointsText, canvas.transform);
                    p.GetComponent<RectTransform>().localPosition = HelperFunctions.objectCameraConvert(transform.position, canvas, cam);
                    p.GetComponent<Text>().text = ((int)(points * ctrl.multiplier)).ToString();
                }
            }
            HelperFunctions.playSound(ref _audioSource, deathSound); //i think popping sound because the enemy dies before sound finishes

            isDead = true;
            invincible = true;



            if (GetComponent<Simple_Movement>() != null)
            {
                GetComponent<Simple_Movement>().stopMovement();
            }
            foreach (GameObject spriteObject in spriteObjects)
            {
                if (spriteObject.GetComponent<ConstantRotation>() != null)
                {
                    spriteObject.GetComponent<ConstantRotation>().setSpeed(0);
                }
                if (spriteObject.GetComponent<ColorLerp>() != null)
                {
                    ColorLerp colorLerp = spriteObject.GetComponent<ColorLerp>(); // Fade to transparent
                    colorLerp.startColor = deathColor;
                    colorLerp.endColor = new Color(deathColor.r, deathColor.g, deathColor.b, 0);
                    colorLerp.startColorChange();
                }
                if (spriteObject.GetComponent<ColorOscillationWithSprites>() != null)
                {
                    spriteObject.GetComponent<ColorOscillationWithSprites>().stopColorChange();
                }
            }
            Destroy(gameObject, 1);
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
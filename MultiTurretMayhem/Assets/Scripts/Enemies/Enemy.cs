using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Enemy : MonoBehaviour
{
    public int baseHP;
    public int curHP;
    public int damage;
    public int points;
    protected bool isDead;
    public bool invincible = false;
    protected Camera mainCamera;
    private gameController controller;
    protected GameObject spriteObject;
    public GameObject pointsText;
    private GameObject canvas;
    private Camera cam;
    private gameController ctrl;
    public AudioClip deathSound;
    protected AudioSource _audioSource;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        ctrl = GameObject.Find("GameController").GetComponent<gameController>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void baseStart()
    {
        //Put this in Start() in all Enemy children
        spriteObject = transform.GetChild(0).gameObject;                    //This object has the spriterenderer, colorLerp, and constantrotation scripts
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        isDead = false;
        _audioSource.clip = deathSound;
    }
    public void baseUpdate()
    {
        //Put this in Update() in all Enemy children

    }
    public void onPlayerHit()
    {
        doDamage(damage);
        Destroy(gameObject);
    }
    public void doDamage(int damage)
    {
        //ToDo: do damage to player, value passed in by child enemy
        controller.damagePlayer(damage);
        ctrl.multiplier = 1;
    }
    public virtual void takeDamage(int damage, Color deathColor)
    {
        //override this method if an enemy has special effects when hit by laser
        //Color parameter is necessary so that the death animation knows what color to use
        curHP -= damage;
        if(curHP <= 0)
        {
            die(deathColor, false);
        }
    }
    public bool isOnScreen()
    {
        Vector3 screenCoords = mainCamera.WorldToViewportPoint(transform.position);     //object is on screen if 0<x<1 and 0<y<1
        return screenCoords.x <= 1.1 && screenCoords.x >= -0.1 && screenCoords.y <= 1.1 && screenCoords.y >= -0.1;  
        //10% buffer for player convenience
    }
    public void die(Color deathColor, bool byBomb)
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
            
            if(GetComponent<Simple_Movement>() != null)
            {
                GetComponent<Simple_Movement>().stopMovement();
            }
            if(spriteObject.GetComponent<ConstantRotation>() != null)
            {
                spriteObject.GetComponent<ConstantRotation>().setSpeed(0);

            }



            ColorLerp colorLerp = spriteObject.GetComponent<ColorLerp>();
            colorLerp.startColor = deathColor;
            colorLerp.endColor = new Color(deathColor.r, deathColor.g, deathColor.b, 0);
            colorLerp.startColorChange();
            Destroy(gameObject, colorLerp.initialDelay+colorLerp.duration);
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onPlayerHit();
        }
    }
}

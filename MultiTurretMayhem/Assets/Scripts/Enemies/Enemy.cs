using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    public int baseHP;
    public int curHP;
    public int damage;
    public int points;
    private bool isDead;
    public bool invincible = false;
    protected Camera mainCamera;
    private gameController controller;
    public void baseStart()
    {
        //Put this in Start() in all Enemy children
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        isDead = false;
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
    }
    public virtual void takeDamage(int damage, Color deathColor)
    {
        //override this method if an enemy has special effects when hit by laser
        //Color parameter is necessary so that the death animation knows what color to use
        curHP -= damage;
        if(curHP <= 0)
        {
            die(deathColor);
        }
    }
    public bool isOnScreen()
    {
        Vector3 screenCoords = mainCamera.WorldToViewportPoint(transform.position);     //object is on screen if 0<x<1 and 0<y<1
        return screenCoords.x <= 1.1 && screenCoords.x >= -0.1 && screenCoords.y <= 1.1 && screenCoords.y >= -0.1;  
        //10% buffer for player convenience
    }
    public void die(Color deathColor)
    {
        if (!isDead)
        {
            controller.addPoints(points);
            isDead = true;
            invincible = true;
            
            if(GetComponent<Simple_Movement>() != null)
            {
                GetComponent<Simple_Movement>().stopMovement();
            }
            if(GetComponent<ConstantRotation>() != null)
            {
                GetComponent<ConstantRotation>().setSpeed(0);

            }
            ColorLerp colorLerp = GetComponent<ColorLerp>();
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

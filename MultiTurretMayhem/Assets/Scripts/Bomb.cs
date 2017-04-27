using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    // Use this for initialization
    public float growthRate;
    public float growthAcceleration;
    public float initialDelay;
    private Vector3 growthVector;
    private Vector3 accelVector;
    public Color color;
    private LineRenderer[] shapeRenderers;
	void Start () {
        growthVector = new Vector3(growthRate, growthRate, 0);
        accelVector = new Vector3(growthAcceleration, growthAcceleration, 0);
        shapeRenderers = gameObject.GetComponentsInChildren<LineRenderer>();
        for(int i = 0; i < shapeRenderers.Length; i++)
        {
            shapeRenderers[i].startColor = color;
            shapeRenderers[i].endColor = color;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(initialDelay > 0)
        {
            initialDelay -= Time.deltaTime;
        } else
        {
            transform.localScale += growthVector * Time.deltaTime;
            growthVector += accelVector * Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
            coll.gameObject.GetComponent<Enemy>().die();
    }

}

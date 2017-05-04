using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombOrbit : MonoBehaviour {

    // Use this for initialization
    private int bombs;
    private gameController controller;
    private List<Transform> bombPips;
    public float radius;
    private bool gameIsOver;
	void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        bombs = controller.bombs;
        gameIsOver = false;
        bombPips = getBombPips();
        updateLocations(bombs);
	}
	
	// Update is called once per frame
	void Update () {
		if(bombs != controller.bombs)
        {
            bombs = controller.bombs;
            updateLocations(bombs);
        }
        if(!gameIsOver && controller.getGameState() == 2)  //2 == game over
        {
            gameIsOver = true;
            //only run this code once because it's inefficient
            for(int i = 0; i < bombPips.Count; i++)
            {
                if (bombPips[i].gameObject.activeSelf)
                {
                    ColorLerp[] squares = bombPips[i].gameObject.GetComponentsInChildren<ColorLerp>();
                    for (int j = 0; j < squares.Length; j++)
                    {
                        squares[j].startColorChange();
                    }
                }
                
            }
        }
	}
    private List<Transform> getBombPips()
    {
        List<Transform> children = new List<Transform>();
        foreach(Transform child in transform)
        {
            children.Add(child);
        }
        return children;
    }
    private void updateLocations(int num)
    {
        //Error if there aren't enough bombpip objects in the list
        for(int i = 0; i < bombPips.Count; i++)
        {
            bombPips[i].gameObject.SetActive(false);
            //turn off all bomb pips
        }
        float step = Mathf.PI * 2 / num;
        for (int i = 0; i < num; i++)
        {
            float x = radius * Mathf.Cos(step * i);
            float y = radius * Mathf.Sin(step * i);
            bombPips[i].gameObject.SetActive(true); //enable necessary bombpips
            bombPips[i].position = new Vector3(x, y, 0);
        }
    }
    
}

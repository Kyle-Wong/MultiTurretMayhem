using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    // Use this for initialization
    public enum TurretsAvailable
    {
        left,
        right,
        both
    }
    public int levelDuration;
    private GameObject[] turrets;
    public TurretsAvailable turretsAvailable = TurretsAvailable.both;
    public GameObject[] disabledUI;
	void Start () {
        turrets = GameObject.FindGameObjectsWithTag("Turret");
        disableUI(disabledUI);
        setTurrets(turretsAvailable);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void disableUI(GameObject[] list)
    {
        for(int i = 0; i < list.Length; i++)
        {
            list[i].SetActive(false);
        }
    }
    private void setTurrets(TurretsAvailable x)
    {
        switch (x)
        {
            case (TurretsAvailable.left):
                for(int i = 0; i< turrets.Length; i++)
                {
                    if(turrets[i].GetComponent<TurretOneScript>() != null)
                    {
                        turrets[i].SetActive(false);
                    }
                }
                break;
            case (TurretsAvailable.right):
                for (int i = 0; i < turrets.Length; i++)
                {
                    if (turrets[i].GetComponent<TurretTwoScript>() != null)
                    {
                        turrets[i].SetActive(false);
                    }
                }
                break;
            case (TurretsAvailable.both):
                break;
        }
    }
   
}

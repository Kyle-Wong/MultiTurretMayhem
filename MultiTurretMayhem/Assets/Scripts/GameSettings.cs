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
    public enum TurretRestriction
    {
        no_restriction,
        left,
        right,

    }
    public int levelDuration;
    private GameObject[] turrets;
    public TurretsAvailable turretsAvailable = TurretsAvailable.both;
    public TurretRestriction leftTurretRestr = TurretRestriction.no_restriction;
    public TurretRestriction rightTurretRestr = TurretRestriction.no_restriction;
    public int startingBombs = 3;
    public GameObject[] enabledUI;
	void Start () {
        turrets = GameObject.FindGameObjectsWithTag("Turret");
        enableUI(enabledUI);
        setTurrets(turretsAvailable, leftTurretRestr, rightTurretRestr);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void enableUI(GameObject[] list)
    {
        for(int i = 0; i < list.Length; i++)
        {
            list[i].SetActive(true);
        }
    }
    private void setTurrets(TurretsAvailable available, TurretRestriction lRestriction, TurretRestriction rRestriction)
    {
        const int LEFT = 0;
        const int RIGHT = 1;
        for(int i = 0; i < turrets.Length; i++)
        {
            if ((int)turrets[i].GetComponent<Turret>().side == RIGHT)
            {
                turrets[i].GetComponent<Turret>().setRestriction((int)rRestriction);
            }
            if ((int)turrets[i].GetComponent<Turret>().side == LEFT)
            {
                turrets[i].GetComponent<Turret>().setRestriction((int)lRestriction);
            }
        }
        switch (available)
        {
            case (TurretsAvailable.left):
                for(int i = 0; i< turrets.Length; i++)
                {
                    if ((int)turrets[i].GetComponent<Turret>().side == RIGHT)
                    {
                        turrets[i].SetActive(false);
                    }
                }
                break;
            case (TurretsAvailable.right):
                for (int i = 0; i < turrets.Length; i++)
                {
                    if ((int)turrets[i].GetComponent<Turret>().side == LEFT)
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

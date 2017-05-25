using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChildMakeDontDestroyOnLoad : MonoBehaviour {

    static int starCount = -1;             //only correct for stars.  I don't know how to make this generic,
                                                //but only calculating the number of duplicates once per scene load
                                                //is far better than .findbytag-ing a 200 length array 200 times
    public int legalDuplicateCount = 1;     //if there is more than legalDuplicateCount objects with this tag (including this object), destroy this object
    public string[] sceneBlackList;     //Destroy this object if it exists in any of these scenes
    private bool willPersist = false;
    void Awake()
    {
        if (starCount == -1)
        {
            starCount = GameObject.FindGameObjectsWithTag(gameObject.tag).Length;
        }
        if (isInBlackListedScene(SceneManager.GetActiveScene()))
        {
            Destroy(gameObject);                //if in a blacklisted scene, destroy this object
        }
        if (starCount > legalDuplicateCount)
        {
            Destroy(gameObject);        //if there is more than legalDuplicateCount objects with this tag (including this object), destroy this object
        }
        //IMPORTANT--Child gameobject CANNOT be made to dontdestroyonload
        //They must have no parents for this to work
    }

    // Update is called once per frame
    void Update()
    {
        if(!willPersist && transform.parent == null)
        {
            willPersist = true;
            DontDestroyOnLoad(gameObject);
        }
    }
    private bool isInBlackListedScene(Scene scene)
    {
        foreach (string s in sceneBlackList)
        {
            if (s.Equals(scene.name))
                return true;
        }
        return false;
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        starCount = -1;
        if (isInBlackListedScene(scene))
        {
            Destroy(gameObject);
        }
    }
}

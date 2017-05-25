using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MakeDontDestroyOnLoad : MonoBehaviour {

    // Use this for initialization
    public int legalDuplicateCount = 1;     //if there is more than legalDuplicateCount objects with this tag (including this object), destroy this object
    public string[] sceneBlackList;     //Destroy this object if it exists in any of these scenes

	void Awake () {
        if (isInBlackListedScene(SceneManager.GetActiveScene()))
        {
            Destroy(gameObject);                //if in a blacklisted scene, destroy this object
        }
        if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > legalDuplicateCount)
        {
            Destroy(gameObject);        //if there is more than legalDuplicateCount objects with this tag (including this object), destroy this object
        } else
        {
            DontDestroyOnLoad(gameObject);      //if this is not in a blacklisted scene
        }
    }
	
	// Update is called once per frame
	void Update()
    {

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
        if (isInBlackListedScene(scene))
        {
            Destroy(gameObject);
        }
    }
    
}

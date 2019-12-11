using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    
    GameObject winObj;
    int stars;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        winObj = FindInactiveObjectByName("Win");
        stars = 0;
    }

    public void LevelWin()
    {
        if (winObj)
            winObj.SetActive(true);
    }

    public void StarsCount()
    {
        stars++;
    }

    public int GetStars()
    {
        return stars;
    }

    //----------------------------------------------------------------------------
    //Finds inactive GameObjects by name.
    //----------------------------------------------------------------------------
    public GameObject FindInactiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //单例模式
    public static LevelManager instance;
    private GameObject sceneManager;
    private Transform playerStart;
    private GameObject player;

    private void Awake()
    {
        sceneManager = GameObject.Find("SceneManager");
        if(sceneManager!=null)
        {
            instance = sceneManager.GetComponent<LevelManager>();
            if(instance==null)
            {
                instance = sceneManager.AddComponent<LevelManager>();
            }
        }
       else
        {
            sceneManager = new GameObject("SceneManager");
            instance = sceneManager.AddComponent<LevelManager>();
        }

        playerStart = GameObject.Find("PlayerStart").GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void InitPlayer()
    {

    }
}

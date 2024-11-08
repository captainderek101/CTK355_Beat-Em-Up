using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject playerObject;

    private Vector3 checkPointPosToLoad;
    private bool useCheckPoint = false;
    private int previousLevel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        playerObject = FindObjectOfType<PlayerInputController>().gameObject;
        previousLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += RespawnAtCheckpoint;
        //SceneManager.activeSceneChanged
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        checkPointPosToLoad = pos;
        useCheckPoint = true;
    }

    private void RespawnAtCheckpoint(Scene scene, LoadSceneMode mode)
    {
        playerObject = FindObjectOfType<PlayerInputController>().gameObject;
        if (useCheckPoint && scene.buildIndex == previousLevel)
        {
            Debug.Log("checkpoint used");
            playerObject.transform.position = checkPointPosToLoad;
        }
        else if (useCheckPoint)
        {
            useCheckPoint = false;
        }
        previousLevel = scene.buildIndex;
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    RespawnAtCheckpoint(SceneManager.GetSceneAt(level), LoadSceneMode.Additive);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] playerObjects;

    private Vector3 checkPointPosToLoad;
    private bool useCheckPoint = false;
    private int previousLevel;

    public bool inFirstLoadedScene = true;

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
        LoadPlayer();
        previousLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += RespawnAtCheckpoint;
        //SceneManager.activeSceneChanged
    }

    private void Start()
    {
        LoadPlayer(); // load player stats now that they can be found
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        checkPointPosToLoad = pos;
        useCheckPoint = true;
    }

    private void RespawnAtCheckpoint(Scene scene, LoadSceneMode mode)
    {
        LoadPlayer();
        if (useCheckPoint && scene.buildIndex == previousLevel)
        {
            foreach(GameObject playerObject in playerObjects)
            {
                playerObject.transform.position = checkPointPosToLoad;
            }
        }
        else if (useCheckPoint)
        {
            useCheckPoint = false;
        }
        previousLevel = scene.buildIndex;
    }

    private void LoadPlayer()
    {
        var playerMovementControllers = FindObjectsOfType<PlayerMovementController>();
        playerObjects = new GameObject[playerMovementControllers.Length];
        for (int i = 0; i < playerMovementControllers.Length; i++)
        {
            playerObjects[i] = playerMovementControllers[i].gameObject;
        }
        if (PlayerStatManager.Instance != null)
        {
            foreach (GameObject playerObject in playerObjects)
            {
                PlayerStatManager.Instance.ApplyPlayerStats(playerObject);
            }
        }
    }
}

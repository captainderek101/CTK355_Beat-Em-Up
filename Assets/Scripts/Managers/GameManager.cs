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
        playerObject = FindObjectOfType<PlayerMovementController>().gameObject;
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
            Debug.Log("checkpoint used");
            playerObject.transform.position = checkPointPosToLoad;
        }
        else if (useCheckPoint)
        {
            useCheckPoint = false;
        }
        previousLevel = scene.buildIndex;
    }

    public void EnableOrDisablePlayer(bool enable)
    {
        //playerObject.GetComponent<PlayerMovementController>().primaryMovementEnabled = enable;
        //playerObject.GetComponent<PlayerAttackController>().readyToAttack = enable;
    }

    private void LoadPlayer()
    {
        playerObject = FindObjectOfType<PlayerMovementController>().gameObject;
        if (playerObject != null)
        {
            PlayerStatManager.Instance.ApplyPlayerStats(playerObject);
        }
    }
}

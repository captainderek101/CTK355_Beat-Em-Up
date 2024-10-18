using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void TransitionEvent();
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    public DeathEvent transition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().buildIndex));
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public void LoadSceneAsync(int buildIndex)
    {
        StartCoroutine(LoadSceneCoroutine(buildIndex));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
    private IEnumerator LoadSceneCoroutine(int buildIndex)
    {
        yield return SceneManager.LoadSceneAsync(buildIndex);
    }
}

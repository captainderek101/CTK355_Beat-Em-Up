using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
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

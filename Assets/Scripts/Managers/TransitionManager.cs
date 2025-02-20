using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public delegate void TransitionEvent(float duration);
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    public TransitionEvent transitionEvent;

    public float defaultTransitionTime = 2f;

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
        StartCoroutine(LoadSceneCoroutine(SceneManager.GetActiveScene().buildIndex, defaultTransitionTime));
    }
    public void LoadSceneAsync(string sceneName, float transitionTime = -1)
    {
        if (transitionTime < -0.01)
        {
            transitionTime = defaultTransitionTime;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName, transitionTime));
    }
    public void LoadSceneAsync(int buildIndex, float transitionTime = -1)
    {
        if (transitionTime < -0.01)
        {
            transitionTime = defaultTransitionTime;
        }
        StartCoroutine(LoadSceneCoroutine(buildIndex, transitionTime));
    }
    private IEnumerator LoadSceneCoroutine(int buildIndex, float transitionTime)
    {
        transitionEvent.Invoke(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.inFirstLoadedScene = false;
        yield return SceneManager.LoadSceneAsync(buildIndex);
    }
    private IEnumerator LoadSceneCoroutine(string sceneName, float transitionTime)
    {
        transitionEvent.Invoke(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        GameManager.Instance.inFirstLoadedScene = false;
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}

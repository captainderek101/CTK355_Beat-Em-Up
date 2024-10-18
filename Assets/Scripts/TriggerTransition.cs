using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransition : MonoBehaviour
{
    [SerializeField] private bool transitionOnTriggerEnter = true;
    [SerializeField] private string sceneToLoad = "";

    private void OnTriggerEnter(Collider other)
    {
        if (transitionOnTriggerEnter && other.transform.IsChildOf(GameManager.Instance.playerObject.transform))
        {
            TransitionManager.Instance.LoadSceneAsync(sceneToLoad);
        }
        else if (other.transform.IsChildOf(GameManager.Instance.playerObject.transform))
        {
            UIManager.Instance.ShowLevelCompleteScreen(sceneToLoad);
        }
    }
}

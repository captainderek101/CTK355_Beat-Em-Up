using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransition : MonoBehaviour
{
    [SerializeField] private string[] scenesToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.playerObject.GetComponentInChildren<Collider>().transform)
        {
            UIManager.Instance.ShowLevelCompleteScreen(scenesToLoad);
        }
    }
}

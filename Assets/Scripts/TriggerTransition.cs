using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTransition : MonoBehaviour
{
    [SerializeField] private StoryProgressionManager.StoryPoint checkpointToComplete;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.playerObject.GetComponentInChildren<Collider>().transform)
        {
            StoryProgressionManager.Instance.SetCheckpoint(checkpointToComplete, true);
            UIManager.Instance.ShowLevelCompleteScreen();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == GameManager.Instance.playerObject.transform)
        {
            StoryProgressionManager.Instance.SetCheckpoint(checkpointToComplete, true);
            UIManager.Instance.ShowLevelCompleteScreen();
        }
    }
}

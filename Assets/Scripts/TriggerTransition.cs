using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerTransition : MonoBehaviour
{
    [SerializeField] private StoryProgressionManager.StoryPoint checkpointToComplete;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(GameManager.Instance.playerObjects
        //    .Where(x => x.transform == other.transform.parent)
        //    .Count());
        if(GameManager.Instance.playerObjects
            .Where(x => x.transform == other.transform.parent)
            .Count() > 0)
        {
            StoryProgressionManager.Instance.SetCheckpoint(checkpointToComplete, true);
            UIManager.Instance.ShowLevelCompleteScreen();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.playerObjects
            .Where(x => x.transform == collision.transform)
            .Count() > 0)
        {
            StoryProgressionManager.Instance.SetCheckpoint(checkpointToComplete, true);
            UIManager.Instance.ShowLevelCompleteScreen();
        }
    }
}

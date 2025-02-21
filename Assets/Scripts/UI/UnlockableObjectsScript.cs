using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableObjectsScript : MonoBehaviour
{
    [SerializeField] private List<UnlockRequirements> managedObjects;

    public void EvaluateUnlockConditions()
    {
        foreach (UnlockRequirements requirement in managedObjects)
        {
            requirement.lockedObject.SetActive(StoryProgressionManager.Instance.GetCheckpoint(requirement.requiredCheckpoint));
        }
    }

    private void OnEnable()
    {
        EvaluateUnlockConditions();
    }

    [Serializable]
    public struct UnlockRequirements
    {
        public GameObject lockedObject;
        public StoryProgressionManager.StoryPoint requiredCheckpoint;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistUponParentDeath : MonoBehaviour
{
    private void Start()
    {
        HealthController parentHealthController = GetComponentInParent<HealthController>();
        if (parentHealthController != null)
        {
            //Debug.Log("found parent health controller");
            parentHealthController.deathEvents.AddListener((GameObject source) =>
            {
                transform.parent = null;
            });
        }
    }
}

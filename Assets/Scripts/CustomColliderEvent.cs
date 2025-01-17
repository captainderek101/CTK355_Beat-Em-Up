using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomColliderEvent : MonoBehaviour
{
    [SerializeField] private ColliderEventType eventType;
    [SerializeField] private bool useTrigger = true;

    private void StartEvent(Collider collision)
    {
        switch (eventType)
        {
            case ColliderEventType.OpenUpgradeShop:
                if(collision.tag == "Player")
                {
                    UIManager.Instance.OpenUpgradeShop();
                }
                break;
            default: // do nothing
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (useTrigger)
        {
            StartEvent(collision);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger)
        {
            StartEvent(collision.collider);
        }
    }

    public enum ColliderEventType
    {
        OpenUpgradeShop
    }
}

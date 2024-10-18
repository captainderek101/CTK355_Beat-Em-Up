using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Health health;

    private void Start()
    {
        health = GameManager.Instance.playerObject.GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            health.UpdateCheckpoint(transform.position);
        }
    }
}

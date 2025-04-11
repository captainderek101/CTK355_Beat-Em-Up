using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string playerTag = "Player";
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            GameManager.Instance.UpdateCheckpoint(transform.position);
            PlayerInputController.Instance.RespawnDeadPlayers();
        }
    }
}

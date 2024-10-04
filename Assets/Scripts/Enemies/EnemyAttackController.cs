using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackController
{
    [SerializeField] private float maxAttackDistance = 0.1f;

    private Transform playerTransform;

    private Vector3 toPlayer = Vector3.zero;

    private void Start()
    {
        if (GameManager.Instance.playerObject != null)
        {
            playerTransform = GameManager.Instance.playerObject.transform;
        }
        else
        {
            Debug.LogError("Game State Controller's player is null!");
        }
    }

    private void FixedUpdate()
    {
        toPlayer = playerTransform.transform.position - transform.position;
        if(toPlayer.magnitude < maxAttackDistance)
        {
            Attack();
        }
    }
}

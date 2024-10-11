using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackController
{
    [SerializeField] private float maxAttackDistance = 0.1f;
    [SerializeField] private bool attacksTargetPlayer = true;

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
        if(playerTransform == null)
        {
            return;
        }
        toPlayer = playerTransform.transform.position - transform.position;
        if(toPlayer.magnitude < maxAttackDistance)
        {
            if(attacksTargetPlayer)
            {
                AttackTargeted(playerTransform);
            }
            else
            {
                Attack();
            }
        }
    }
}

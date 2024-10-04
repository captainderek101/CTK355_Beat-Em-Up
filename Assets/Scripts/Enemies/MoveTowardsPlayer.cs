using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private EnemyMovementController controller;
    private Transform playerTransform;

    [SerializeField] private float distanceToStop = 2f;

    private Vector3 moveDirection = Vector2.zero;

    void Start()
    {
        controller = GetComponent<EnemyMovementController>();
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
        moveDirection = transform.position - playerTransform.position;
        moveDirection.y = 0;
        if (moveDirection.magnitude < distanceToStop )
        {
            moveDirection = Vector3.zero;
        }
        controller.SetMoveDirection(moveDirection.normalized);
    }
}

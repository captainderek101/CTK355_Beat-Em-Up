using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTowardsPlayer : MonoBehaviour
{
    private EnemyMovementController controller;
    private Transform playerTransform;

    [SerializeField] private float distanceToStop = 2f;
    [SerializeField] private float targetDistanceFromPlayer = 2f;
    [SerializeField] private float distanceToKeepFromEnemies = 2f;
    [SerializeField] private int fixedUpdateClockCycle = 30;
    private int currentClockCycle = 0;

    private Vector3 moveDirection = Vector2.zero;
    private Vector3 targetPosition = Vector2.zero;

    private const int enemyLayer = 1 << 6;

    private void Start()
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
        fixedUpdateClockCycle = Mathf.RoundToInt(fixedUpdateClockCycle * Random.Range(0.8f, 1.2f));
    }

    private void FixedUpdate()
    {
        if (currentClockCycle == 0)
        {
            if (playerTransform == null)
            {
                return;
            }
            targetPosition = playerTransform.position;
            if ((playerTransform.position - transform.position).x > 0)
            {
                targetPosition.x += targetDistanceFromPlayer;
            }
            else
            {
                targetPosition.x -= targetDistanceFromPlayer;
            }
            moveDirection = targetPosition - transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, distanceToKeepFromEnemies, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                // check if other enemy is "in the direction of" player
                if (Mathf.Abs(Vector3.Angle(moveDirection, hitCollider.transform.position - transform.position)) < 89f)
                {
                    bool canIContinue = EnemyManager.Instance.CanIContinue(gameObject, hitCollider.gameObject);
                    if (!canIContinue)
                    {
                        moveDirection = moveDirection - (hitCollider.transform.position - transform.position);
                        //Debug.Log(gameObject + " was designated as the virgin");
                        break;
                    }
                }
            }
            moveDirection.y = 0;
            if (moveDirection.magnitude < distanceToStop)
            {
                moveDirection = Vector3.zero;
            }
            controller.SetMoveDirection(moveDirection.normalized);
            currentClockCycle = fixedUpdateClockCycle;
        }
        else
        {
            currentClockCycle--;
        }
    }
}

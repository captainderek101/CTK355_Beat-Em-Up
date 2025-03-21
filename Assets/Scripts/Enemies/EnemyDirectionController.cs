using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(EnemyAttackController))]
[RequireComponent(typeof(EnemyAggroController))]
public class EnemyDirectionController : MonoBehaviour
{
    [SerializeField] private DirectionType type;
    private MovementController movementController;
    private EnemyAttackController attackController;
    private EnemyAggroController aggroController;
    private Transform playerTransform;
    private bool facingRight = true;

    private void Start()
    {
        TryGetComponent(out movementController);
        TryGetComponent(out attackController);
        TryGetComponent(out aggroController);
        playerTransform = GameManager.Instance.playerObjects[Random.Range(0, GameManager.Instance.playerObjects.Length)].transform;
    }

    private void FixedUpdate()
    {
        switch (type)
        {
            case DirectionType.FollowsMovement:
                FollowMovement();
                break;
            case DirectionType.FollowsPlayerWhileAggroed:
                if(aggroController.aggroed)
                {
                    FollowPlayer();
                }
                else
                {
                    FollowMovement();
                }
                break;
            default:
                Debug.LogWarning(nameof(EnemyDirectionController) + " attached to " + gameObject + " fell through to default");
                break;
        }
    }

    private void FollowMovement()
    {
        if (facingRight && ((EnemyMovementController)movementController).movementInput.x < 0 && movementController.primaryMovementEnabled)
        {
            facingRight = false;
            attackController.FacingLeftOrRight(false);
        }
        else if (!facingRight && ((EnemyMovementController)movementController).movementInput.x > 0 && movementController.primaryMovementEnabled)
        {
            facingRight = true;
            attackController.FacingLeftOrRight(true);
        }
    }

    private void FollowPlayer()
    {
        if (facingRight && (playerTransform.position.x < transform.position.x) && movementController.primaryMovementEnabled)
        {
            facingRight = false;
            attackController.FacingLeftOrRight(false);
        }
        else if (!facingRight && (playerTransform.position.x > transform.position.x) && movementController.primaryMovementEnabled)
        {
            facingRight = true;
            attackController.FacingLeftOrRight(true);
        }
    }

    public enum DirectionType
    {
        FollowsMovement,
        FollowsPlayerWhileAggroed
    }
}

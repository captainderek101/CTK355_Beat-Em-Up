using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MovementController
{
    public Vector3 movementInput = Vector3.zero;
    private Vector3 realMovement = Vector3.zero;
    private void FixedUpdate()
    {
        realMovement = Vector3.zero;
        realMovement += rightDirection * movementInput.x * horizontalMoveSpeed;
        realMovement += upDirection * movementInput.z * verticalMoveSpeed;
        if (primaryMovementEnabled)
        {
            gameObject.transform.position += realMovement * Time.fixedDeltaTime;
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        movementInput = direction;
    }
}

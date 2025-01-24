using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MovementController
{
    public Vector3 movementInput = Vector3.zero;
    private Vector3 realMovement = Vector3.zero;

    public string walkingAnimationBool = "Walking";

    private void FixedUpdate()
    {
        realMovement = Vector3.zero;
        realMovement += rightDirection * movementInput.x * horizontalMoveSpeed;
        realMovement += upDirection * movementInput.z * verticalMoveSpeed;
        if (primaryMovementEnabled && notBusy)
        {
            gameObject.transform.position += realMovement * Time.fixedDeltaTime;
            if (realMovement.magnitude > 0.01f)
            {
                if (!animationController.GetBool(walkingAnimationBool))
                {
                    animationController.SetBool(walkingAnimationBool, true);
                }
            }
            else if (animationController.GetBool(walkingAnimationBool))
            {
                animationController.SetBool(walkingAnimationBool, false);
            }
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        movementInput = direction;
    }
}

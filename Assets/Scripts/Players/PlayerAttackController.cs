using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : AttackController
{
    private PlayerInputs.PlayerActions actions;

    private Vector2 movementInput;

    private void Start()
    {
        actions = PlayerInputController.Instance.inputActions.Player;
    }

    private void Update()
    {
        // TODO: holding down the attack key should let you attack repeatedly!!
        if (actions.LightAttack.WasPressedThisFrame())
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        movementInput = actions.Move.ReadValue<Vector2>();
        if(facingRight && movementInput.x < 0)
        {
            facingRight = false;
        }
        else if(!facingRight && movementInput.x > 0)
        {
            facingRight = true;
        }
    }
}

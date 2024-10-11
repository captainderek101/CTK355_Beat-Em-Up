using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class PlayerAttackController : AttackController
{
    private PlayerInputs.PlayerActions actions;
    private AudioPlayer audioPlayer;

    private Vector2 movementInput;

    private const string lightAttackAudioName = "lightAttack";

    private void Start()
    {
        actions = PlayerInputController.Instance.inputActions.Player;
        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Update()
    {
        // TODO: holding down the attack key should let you attack repeatedly!!
        if (actions.LightAttack.WasPressedThisFrame())
        {
            audioPlayer.PlaySound(lightAttackAudioName);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class PlayerAttackController : AttackController
{
    private PlayerInputs.PlayerActions actions;
    private AudioPlayer audioPlayer;

    private Vector2 movementInput;

    private const string lightAttackAttackName = "light";
    private const string lightAttackAudioName = "lightAttack";
    private const string lightAttackAnimationTrigger = "Light Attack";
    private const string strongAttackAttackName = "strong";
    private const string strongAttackAudioName = "strongAttack";
    private const string strongAttackAnimationTrigger = "Strong Attack";

    private void Start()
    {
        actions = PlayerInputController.Instance.inputActions.Player;
        audioPlayer = GetComponent<AudioPlayer>();
        animationController = GetComponent<Animator>();
    }

    private void Update()
    {
        // TODO: holding down the attack key should let you attack repeatedly!!
        if (actions.LightAttack.WasPressedThisFrame())
        {
            bool success = Attack(lightAttackAttackName);
            if (success)
            {
                animationController.SetTrigger(lightAttackAnimationTrigger);
                audioPlayer.PlaySound(lightAttackAudioName);
            }
        }
        else if (actions.StrongAttack.WasPressedThisFrame())
        {
            bool success = Attack(strongAttackAttackName);
            if (success)
            {
                animationController.SetTrigger(strongAttackAnimationTrigger);
                audioPlayer.PlaySound(strongAttackAudioName);
            }
            
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

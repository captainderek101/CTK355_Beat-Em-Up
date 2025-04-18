using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioPlayer))]
public class PlayerAttackController : AttackController
{
    //private PlayerInputs.PlayerActions actions;
    private AudioPlayer audioPlayer;

    private Vector2 movementInput;

    private const string lightAttackAttackName = "light";
    private const string lightAttackAudioName = "lightAttack";
    private const string lightAttackAnimationTrigger = "Light Attack";
    private const string strongAttackAttackName = "strong";
    private const string strongAttackAudioName = "strongAttack";
    private const string strongAttackAnimationTrigger = "Strong Attack";
    private const string abilityAttackName = "ability";
    private const string abilityAudioName = "strongAttack";
    private const string abilityAnimationTrigger = "Ability";
    private const string abilityChargedAudioName = "abilityCharged";

    private int abilityChargeLimit = 10;
    private int currentAbilityCharge;
    private bool abilityReady = false;

    private UIGroupControl abilityUI;

    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        //actions = PlayerInputController.Instance.inputActions.Player;
        audioPlayer = GetComponent<AudioPlayer>();
        animationController = GetComponent<Animator>();
        TryGetComponent(out movementController);
        currentAbilityCharge = 0;
        UpdateAbilityUI();
    }

    private void Update()
    {
        if (playerInput.actions.FindAction("LightAttack").WasPressedThisFrame())
        {
            bool success = Attack(lightAttackAttackName);
            if (success)
            {
                animationController.SetTrigger(lightAttackAnimationTrigger);
                audioPlayer.PlaySound(lightAttackAudioName);
            }
        }
        else if (playerInput.actions.FindAction("StrongAttack").WasPressedThisFrame())
        {
            bool success = Attack(strongAttackAttackName);
            if (success)
            {
                animationController.SetTrigger(strongAttackAnimationTrigger);
                audioPlayer.PlaySound(strongAttackAudioName);
            }
        }
        else if (playerInput.actions.FindAction("Ability").WasPressedThisFrame() && abilityReady)
        {
            bool success = Attack(abilityAttackName);
            if (success)
            {
                animationController.SetTrigger(abilityAnimationTrigger);
                audioPlayer.PlaySound(abilityAudioName);
                abilityReady = false;
                currentAbilityCharge = 0;
                UpdateAbilityUI();
            }
        }
    }

    private void FixedUpdate()
    {
        movementInput = playerInput.actions.FindAction("Move").ReadValue<Vector2>();
        if(facingRight && movementInput.x < 0 && movementController.primaryMovementEnabled && movementController.notBusy)
        {
            facingRight = false;
            billboard.flipX = true;
        }
        else if(!facingRight && movementInput.x > 0 && movementController.primaryMovementEnabled && movementController.notBusy)
        {
            facingRight = true;
            billboard.flipX = false;
        }
    }

    public void SetAbilityChargeLimit(int amount)
    {
        abilityChargeLimit = amount;
        if(currentAbilityCharge >= abilityChargeLimit)
        {
            abilityReady = true;
            audioPlayer.PlaySound(abilityChargedAudioName);
        }
        UpdateAbilityUI();
    }

    public void ChargeAbility(int amount)
    {
        currentAbilityCharge += amount;
        if (currentAbilityCharge >= abilityChargeLimit)
        {
            abilityReady = true;
            audioPlayer.PlaySound(abilityChargedAudioName);
        }
        UpdateAbilityUI();
    }

    private void UpdateAbilityUI()
    {
        if(abilityUI == null)
        {
            abilityUI = UIManager.Instance.abilityHUD;
        }
        abilityUI.SetSliderValue(Mathf.Clamp((float)currentAbilityCharge / abilityChargeLimit, 0, 1));
        abilityUI.SetTextValue((int)Mathf.Min(currentAbilityCharge, abilityChargeLimit) + " / " + abilityChargeLimit);
    }
}

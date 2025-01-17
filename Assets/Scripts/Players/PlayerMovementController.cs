using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
[RequireComponent(typeof(PlayerAttackController))]
public class PlayerMovementController : MovementController
{
    [SerializeField] private AnimationCurve dodgerollSpeedCurve;
    [SerializeField] private float dodgerollDuration = 1.0f;

    [SerializeField] private SpriteRenderer playerBillboard;

    private PlayerInputs.PlayerActions actions;
    private PlayerAttackController attackController;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 realMovement = Vector3.zero;

    private const string walkingAnimationBool = "Walking";
    private const string dodgerollAnimationTrigger = "Dodgeroll";
    private AudioPlayer audioPlayer;
    private const string dodgerollAudioName = "roll";
    private const string walkAudioName = "footstep";
    private const float timeBetweenFootstepSounds = 0.3f;
    private bool walkSoundActive = false;

    private bool disabledBecauseOfSelf = false;

    private new void Start()
    {
        base.Start();
        actions = PlayerInputController.Instance.inputActions.Player;

        audioPlayer = GetComponent<AudioPlayer>();
        attackController = GetComponent<PlayerAttackController>();
    }

    private void Update()
    {
        bool dodgerollPressed = actions.Dodgeroll.WasPressedThisFrame();
        if (dodgerollPressed && primaryMovementEnabled && !disabledBecauseOfSelf)
        {
            StartCoroutine(DodgerollCoroutine());
        }
    }

    private void FixedUpdate()
    {
        movementInput = actions.Move.ReadValue<Vector2>();
        realMovement = Vector3.zero;
        realMovement += rightDirection * movementInput.x * horizontalMoveSpeed;
        realMovement += upDirection * movementInput.y * verticalMoveSpeed;
        if (primaryMovementEnabled && !disabledBecauseOfSelf)
        {
            gameObject.transform.position += realMovement * Time.fixedDeltaTime;
            if (realMovement.magnitude > 0.01f)
            {
                if (!walkSoundActive)
                    StartCoroutine(FootstepCoroutine());
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
        else
        {
            animationController.SetBool(walkingAnimationBool, false);
        }
    }

    private IEnumerator DodgerollCoroutine()
    {
        disabledBecauseOfSelf = true;
        attackController.readyToAttack = false;
        animationController.SetTrigger(dodgerollAnimationTrigger);
        float timeSinceStart = 0;
        float currentSpeed = 0;
        audioPlayer.PlaySound(dodgerollAudioName);
        while (timeSinceStart < dodgerollDuration)
        {
            currentSpeed = dodgerollSpeedCurve.Evaluate(timeSinceStart / dodgerollDuration);
            yield return new WaitForFixedUpdate();
            gameObject.transform.position += realMovement * currentSpeed * Time.fixedDeltaTime;
            timeSinceStart += Time.fixedDeltaTime;
        }
        disabledBecauseOfSelf = false;
        attackController.readyToAttack = true;
    }

    private IEnumerator FootstepCoroutine()
    {
        walkSoundActive = true;
        audioPlayer.PlaySound(walkAudioName);
        yield return new WaitForSeconds(timeBetweenFootstepSounds);
        walkSoundActive = false;
    }
}

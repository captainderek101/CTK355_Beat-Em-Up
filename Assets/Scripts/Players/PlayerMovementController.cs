using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioPlayer))]
[RequireComponent(typeof(PlayerAttackController))]
public class PlayerMovementController : MovementController
{
    [SerializeField] private AnimationCurve dodgerollSpeedCurve;
    [SerializeField] private float dodgerollDuration = 1.0f;

    //private PlayerInputs.PlayerActions actions;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 realMovement = Vector3.zero;

    private const string walkingAnimationBool = "Walking";
    private const string dodgerollAnimationTrigger = "Dodgeroll";
    private AudioPlayer audioPlayer;
    private const string dodgerollAudioName = "roll";
    private const string walkAudioName = "footstep";
    private const float timeBetweenFootstepSounds = 0.3f;
    private bool walkSoundActive = false;

    [SerializeField] private PlayerInput playerInput;

    private new void Start()
    {
        base.Start();

        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Update()
    {
        bool dodgerollPressed = playerInput.actions.FindAction("Dodgeroll").WasPressedThisFrame();
        if (dodgerollPressed && primaryMovementEnabled && notBusy)
        {
            StartCoroutine(DodgerollCoroutine());
        }
    }

    private void FixedUpdate()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0)
        {
            screenPosition.x = 0;
            Vector3 newPosition = transform.position;
            newPosition.x = Camera.main.ScreenToWorldPoint(screenPosition).x;
            transform.position = newPosition;
        }
        else if (screenPosition.x > Camera.main.pixelWidth)
        {
            screenPosition.x = Camera.main.pixelWidth;
            Vector3 newPosition = transform.position;
            newPosition.x = Camera.main.ScreenToWorldPoint(screenPosition).x;
            transform.position = newPosition;
        }

        movementInput = playerInput.actions.FindAction("Move").ReadValue<Vector2>();
        realMovement = Vector3.zero;
        realMovement += rightDirection * movementInput.x * horizontalMoveSpeed;
        realMovement += upDirection * movementInput.y * verticalMoveSpeed;
        if (primaryMovementEnabled && notBusy)
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
        animationController.SetTrigger(dodgerollAnimationTrigger);
        float timeSinceStart = 0;
        float currentSpeed = 0;
        audioPlayer.PlaySound(dodgerollAudioName);
        while (timeSinceStart < dodgerollDuration)
        {
            currentSpeed = dodgerollSpeedCurve.Evaluate(timeSinceStart / dodgerollDuration);
            yield return new WaitForFixedUpdate();
            if(primaryMovementEnabled)
            {
                gameObject.transform.position += realMovement * currentSpeed * Time.fixedDeltaTime;
            }
            timeSinceStart += Time.fixedDeltaTime;
        }
    }

    private IEnumerator FootstepCoroutine()
    {
        walkSoundActive = true;
        audioPlayer.PlaySound(walkAudioName);
        yield return new WaitForSeconds(timeBetweenFootstepSounds);
        walkSoundActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class BossAttackController : AttackController
{
    [SerializeField] private float targetYDistanceFromPlayer = 0.05f;
    [SerializeField] private float targetXDistanceFromPlayer = 0.1f;
    [SerializeField] private float YDistanceNegativeBias = 4;
    [SerializeField] private float XDistanceNegativeBias = 2;

    [SerializeField] private int fixedUpdateClockCycle = 50;
    private int currentClockCycle = 0;

    [HideInInspector] public Transform playerTransform;

    private Vector3 toPlayer = Vector3.zero;

    private const string lightAttackAnimationTrigger = "Light Attack";
    private const string lightAttackAudioName = "attack";

    private AudioPlayer audioPlayer;

    private void Start()
    {
        TryGetComponent(out movementController);
        animationController = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void FixedUpdate()
    {
        if (currentClockCycle == 0)
        {
            if (playerTransform == null)
            {
                return;
            }
            toPlayer = playerTransform.transform.position - transform.position;
            float chanceToAttack = 1;
            if (Mathf.Abs(toPlayer.z) > targetYDistanceFromPlayer + 0.005)
            {
                chanceToAttack -= Mathf.Abs(toPlayer.z) * YDistanceNegativeBias;
            }
            if (Mathf.Abs(toPlayer.x) > targetXDistanceFromPlayer + 0.005)
            {
                chanceToAttack -= Mathf.Abs(toPlayer.x) * XDistanceNegativeBias;
            }
            if (Random.value < chanceToAttack)
            {
                bool success = Attack();
                if (success)
                {
                    animationController.SetTrigger(lightAttackAnimationTrigger);
                    audioPlayer.PlaySound(lightAttackAudioName);
                }
            }
            currentClockCycle = fixedUpdateClockCycle;
        }
        else
        {
            currentClockCycle--;
        }
    }
}

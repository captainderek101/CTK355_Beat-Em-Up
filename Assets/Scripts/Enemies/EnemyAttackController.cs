using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class EnemyAttackController : AttackController
{
    [SerializeField] private float targetYDistanceFromPlayer = 0.05f;
    [SerializeField] private float targetXDistanceFromPlayer = 0.1f;
    [SerializeField] private float YDistanceNegativeBias = 4;
    [SerializeField] private float XDistanceNegativeBias = 2;
    [SerializeField] private bool attacksTargetPlayer = false;
    [SerializeField] private int fixedUpdateClockCycle = 50;
    [SerializeField] private bool billboardFacingRight = false;
    private int currentClockCycle = 0;

    private Transform playerTransform;

    private Vector3 toPlayer = Vector3.zero;
    private EnemyAggroController aggroController;

    private const string lightAttackAnimationTrigger = "Light Attack";
    private const string lightAttackAudioName = "attack";

    [SerializeField] private SpriteRenderer billboard;
    private AudioPlayer audioPlayer;

    private void Start()
    {
        if (GameManager.Instance.playerObject != null)
        {
            playerTransform = GameManager.Instance.playerObject.transform;
        }
        else
        {
            Debug.LogError("Game State Controller's player is null!");
        }
        TryGetComponent(out aggroController);
        TryGetComponent(out movementController);
        animationController = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioPlayer>();

        fixedUpdateClockCycle = Mathf.RoundToInt(fixedUpdateClockCycle * Random.Range(0.8f, 1.2f));
    }

    public void FacingLeftOrRight(bool facingRight)
    {
        this.facingRight = facingRight;
        billboard.flipX = billboardFacingRight ? !facingRight : facingRight;
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
                if (aggroController != null && aggroController.aggroed == false)
                {
                    // Don't attack if not aggroed!
                    return;
                }
                bool success = false;
                if (attacksTargetPlayer)
                {
                    success = AttackTargeted(playerTransform);
                }
                else
                {
                    success = Attack();
                }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackController
{
    [SerializeField] private float maxAttackDistance = 0.1f;
    [SerializeField] private bool attacksTargetPlayer = true;

    private Transform playerTransform;

    private Vector3 toPlayer = Vector3.zero;
    private EnemyAggroController aggroController;

    private const string lightAttackAnimationTrigger = "Light Attack";

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
        animationController = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(playerTransform == null)
        {
            return;
        }
        toPlayer = playerTransform.transform.position - transform.position;
        if(toPlayer.magnitude < maxAttackDistance)
        {
            if(aggroController != null && aggroController.aggroed == false)
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
            }
        }
    }
}

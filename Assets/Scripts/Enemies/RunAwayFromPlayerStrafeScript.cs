using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayFromPlayerStrafeScript : MonoBehaviour
{
    private EnemyMovementController controller;
    private Transform playerTransform;

    [SerializeField] private float minStrafeDistance = 1f;
    [SerializeField] private float minChaseDistance = 2f;
    [SerializeField] private float brainClockCycle = 1f;
    [SerializeField] private float chanceToChangeDirection = 0.1f;
    [SerializeField] private float randomMultiplierMin = 0.9f;
    [SerializeField] private float randomMultiplierMax = 1.1f;

    public bool strafeLeft = false;
    private float currentBrainClock;

    void Start()
    {
        controller = GetComponent<EnemyMovementController>();
        if (GameManager.Instance.playerObject != null)
        {
            playerTransform = GameManager.Instance.playerObject.transform;
        }
        else
        {
            Debug.LogError("Game State Controller's player is null!");
        }
        float randomMultiplier = Random.Range(randomMultiplierMin, randomMultiplierMax);
        brainClockCycle *= randomMultiplier;
        currentBrainClock = 0;

        minStrafeDistance *= randomMultiplier;
        minChaseDistance *= randomMultiplier;
    }

    private void FixedUpdate()
    {
        currentBrainClock -= Time.fixedDeltaTime;
        if(currentBrainClock <= 0)
        {
            SetStrafeDirection();
            Vector3 toPlayer = playerTransform.position - transform.position;
            float distance = toPlayer.magnitude;
            if (distance > minChaseDistance)
            {
                controller.SetMoveDirection(toPlayer.normalized);
            }
            else if (distance < minStrafeDistance)
            {
                controller.SetMoveDirection(-toPlayer.normalized);
            }
            else
            {
                Vector3 newMovement = new Vector3(toPlayer.z, 0, -toPlayer.x);
                if (strafeLeft)
                {
                    newMovement *= -1;
                }
                controller.SetMoveDirection(newMovement.normalized);
            }
            currentBrainClock += brainClockCycle;
        }
    }

    private void SetStrafeDirection()
    {
        bool changeDirection = Random.value <= chanceToChangeDirection;
        if(changeDirection)
        {
            strafeLeft = !strafeLeft;
        }
    }
}

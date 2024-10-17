using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    private EnemyMovementController controller;

    [SerializeField] private float brainClockCycle = 1f;
    [SerializeField] private float chanceToMoveOnClockCycle = 0.1f;
    [SerializeField] private float clockRandomizerAmount = 0.1f;
    [SerializeField] private float minMoveTime = 1f;
    [SerializeField] private float maxMoveTime = 2f;

    private const float directionDeviation = 30f;
    private const float maxDistanceToRandomizeDirection = 0.05f;

    private bool moving = false;
    private float currentBrainClock;
    private float timeToMove;
    private Vector3 moveDirection = Vector2.zero;
    private Vector3 startPoint;

    private void Start()
    {
        controller = GetComponent<EnemyMovementController>();
        float randomMultiplier = Random.Range(1 - clockRandomizerAmount, 1 + clockRandomizerAmount);
        brainClockCycle *= randomMultiplier;
        currentBrainClock = 0;
        timeToMove = 0;
        startPoint = transform.position;
    }

    private void Update()
    {
        currentBrainClock -= Time.deltaTime;
        if (currentBrainClock < 0.01f)
        {
            if (!moving)
            {
                bool startMoving = Random.value < chanceToMoveOnClockCycle;
                if (startMoving)
                {
                    moving = true;
                    timeToMove = Random.Range(minMoveTime, maxMoveTime);
                    Vector3 moveDirection = startPoint - transform.position;
                    if(moveDirection.magnitude < maxDistanceToRandomizeDirection)
                    {
                        moveDirection = Random.insideUnitCircle.normalized;
                    }
                    float directionModifier = Random.Range(-directionDeviation, directionDeviation);
                    moveDirection = Quaternion.Euler(0, directionModifier, 0) * moveDirection;
                    controller.SetMoveDirection(moveDirection.normalized);
                }
            }
            currentBrainClock += brainClockCycle;
        }
        if (moving && timeToMove > 0)
        {
            timeToMove -= Time.deltaTime;
        }
        else if (moving)
        {
            moving = false;
            controller.SetMoveDirection(Vector3.zero);
        }
    }

    private void OnEnable()
    {
        currentBrainClock = 0;
        timeToMove = 0;
        startPoint = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedProjectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve startingSpeedCurve;
    [SerializeField] private float speedCurveDuration = 1.0f;
    [SerializeField] private float timeToDespawn = 1f;
    [SerializeField] private float distanceToDespawn = 20f;
    private float timeAwake = 0;
    private float distanceTraveled;

    [SerializeField] private bool trackTarget = false;
    private Vector3 movement;
    public float currentSpeed = 0;

    public Transform target;

    private void Start()
    {
        movement = target.position - transform.position;
        movement.y = 0;
    }

    private void FixedUpdate()
    {
        if (trackTarget && target != null)
        {
            movement = target.position - transform.position;
            movement.y = 0;
        }
        if (timeAwake < speedCurveDuration + 0.01f)
        {
            currentSpeed = startingSpeedCurve.Evaluate(timeAwake / speedCurveDuration);
        }
        Vector3 deltPos = movement.normalized * currentSpeed * Time.fixedDeltaTime;
        gameObject.transform.position += deltPos;
        timeAwake += Time.fixedDeltaTime;
        distanceTraveled += deltPos.magnitude;
        if(timeAwake > timeToDespawn || distanceTraveled > distanceToDespawn)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    [SerializeField] protected AxisDirection whereIsRight = AxisDirection.PositiveX;
    protected Vector3 rightDirection;
    [SerializeField] protected AxisDirection whereIsUp = AxisDirection.PositiveZ;
    protected Vector3 upDirection;

    public float horizontalMoveSpeed = 1.0f;
    public float verticalMoveSpeed = 1.0f;

    public bool primaryMovementEnabled = true;
    public bool notBusy = true;
    private bool knockbackInterrupted = false;

    [HideInInspector] public Animator animationController;

    protected void Start()
    {
        rightDirection = GetVector3FromEnum(whereIsRight);
        upDirection = GetVector3FromEnum(whereIsUp);
        animationController = GetComponent<Animator>();
    }

    public void ApplyKnockback(AnimationCurve curve, float duration, bool left)
    {
        if(!primaryMovementEnabled)
        {
            knockbackInterrupted = true;
        }
        primaryMovementEnabled = false;
        StartCoroutine(KnockbackCoroutine(curve, duration, left));
    }

    private IEnumerator KnockbackCoroutine(AnimationCurve curve, float duration, bool left)
    {
        float timeSinceStart = 0;
        float currentSpeed = 0;
        Rigidbody rb = GetComponent<Rigidbody>();
        while (timeSinceStart < duration)
        {
            if(knockbackInterrupted)
            {
                break;
            }
            currentSpeed = curve.Evaluate(timeSinceStart / duration);
            if (left)
            {
                currentSpeed *= -1;
            }
            yield return new WaitForFixedUpdate();
            transform.position += Vector3.right * currentSpeed * Time.fixedDeltaTime;
            //rb.MovePosition(transform.position + Vector3.right * currentSpeed * Time.fixedDeltaTime);
            timeSinceStart += Time.fixedDeltaTime;
        }
        if (!knockbackInterrupted)
        {
            primaryMovementEnabled = true;
        }
        knockbackInterrupted = false;
    }

    private Vector3 GetVector3FromEnum(AxisDirection direction)
    {
        switch (direction)
        {
            case AxisDirection.PositiveX:
                return Vector3.right;
            case AxisDirection.NegativeX:
                return Vector3.left;
            case AxisDirection.PositiveZ:
                return Vector3.forward;
            case AxisDirection.NegativeZ:
                return Vector3.back;
            case AxisDirection.PositiveY:
                return Vector3.up;
            case AxisDirection.NegativeY:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }

    public enum AxisDirection
    {
        PositiveX,
        NegativeX,
        PositiveZ,
        NegativeZ,
        PositiveY,
        NegativeY
    }
}

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

    [SerializeField] protected float horizontalMoveSpeed = 1.0f;
    [SerializeField] protected float verticalMoveSpeed = 1.0f;

    protected bool primaryMovementEnabled = true;

    [HideInInspector] public Animator animationController;

    protected void Start()
    {
        rightDirection = GetVector3FromEnum(whereIsRight);
        upDirection = GetVector3FromEnum(whereIsUp);
        animationController = GetComponent<Animator>();
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

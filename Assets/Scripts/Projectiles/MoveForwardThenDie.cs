using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardThenDie : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float distanceToDespawn = 20f;
    private Vector3 direction;
    private float distanceTraveled;

    private void Awake()
    {
        direction = transform.right;
    }

    public void ChangeDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        Vector3 deltPos = direction.normalized * speed * Time.fixedDeltaTime;
        gameObject.transform.position += deltPos;
        distanceTraveled += deltPos.magnitude;
        if (distanceTraveled + 0.001f > distanceToDespawn)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

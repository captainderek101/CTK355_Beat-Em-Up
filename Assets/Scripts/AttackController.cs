using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackCooldown = 0.5f;

    protected bool readyToAttack = true;
    protected bool facingRight = true;

    public bool Attack()
    {
        StartCoroutine(AttackCoroutine());
        return readyToAttack;
    }

    private IEnumerator AttackCoroutine()
    {
        if (readyToAttack)
        {
            readyToAttack = false;

            Quaternion newRotation = transform.rotation;
            if (!facingRight)
            {
                newRotation *= Quaternion.Euler(0, 180, 0);
            }
            Instantiate(attackPrefab, transform.position, newRotation);

            yield return new WaitForSeconds(attackCooldown);
            readyToAttack = true;
        }
    }
}

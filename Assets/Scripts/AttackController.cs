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
        StartCoroutine(AttackCoroutine(null));
        return readyToAttack;
    }

    public bool AttackTargeted(Transform target)
    {
        StartCoroutine(AttackCoroutine(target));
        return readyToAttack;
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        if (readyToAttack)
        {
            readyToAttack = false;

            Quaternion newRotation = transform.rotation;
            if (!facingRight)
            {
                newRotation *= Quaternion.Euler(0, 180, 0);
            }
            GameObject projectile = Instantiate(attackPrefab, transform.position, newRotation);
            if(target != null && projectile.TryGetComponent<TargetedProjectile>(out TargetedProjectile targeting))
            {
                targeting.target = target;
            }

            yield return new WaitForSeconds(attackCooldown);
            readyToAttack = true;
        }
    }
}

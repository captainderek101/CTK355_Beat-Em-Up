using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MovementController))]
public class AttackController : MonoBehaviour
{
    [SerializeField] private NamedAttack[] attackPrefabs;
    [SerializeField] private float attackCooldown = 0.5f;

    public bool readyToAttack = true;
    protected bool diabledBecauseOfSelf = false;
    protected bool facingRight = true;

    [HideInInspector] public Animator animationController;
    [HideInInspector] public MovementController movementController;

    public bool Attack()
    {
        return AttackTargeted(null, attackPrefabs[0].name);
    }
    public bool Attack(string attackName)
    {
        return AttackTargeted(null, attackName);
    }
    public bool AttackTargeted(Transform target)
    {
        return AttackTargeted(target, attackPrefabs[0].name);
    }
    public bool AttackTargeted(Transform target, string attackName)
    {
        bool wasReady = readyToAttack && !diabledBecauseOfSelf;
        StartCoroutine(AttackCoroutine(target, attackPrefabs.Where(x => x.name == attackName).First().prefab));
        return wasReady;
    }

    private IEnumerator AttackCoroutine(Transform target, GameObject attackPrefab)
    {
        if (readyToAttack && !diabledBecauseOfSelf)
        {
            diabledBecauseOfSelf = true;
            //movementController.primaryMovementEnabled = false;

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
            diabledBecauseOfSelf = false;
            //movementController.primaryMovementEnabled = true;
        }
    }

    [Serializable]
    private struct NamedAttack
    {
        public string name;
        public GameObject prefab;
    }
}

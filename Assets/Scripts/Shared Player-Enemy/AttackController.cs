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

    public bool readyToAttack = true;
    public bool notBusy = true;
    protected bool facingRight = true;

    [HideInInspector] public Animator animationController;
    [HideInInspector] public MovementController movementController;

    public float damageMultiplier = 1.0f;

    public SpriteRenderer billboard;
    public bool billboardFacingRight = false;

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
        bool wasReady = readyToAttack && notBusy;
        AttackCoroutine(target, attackPrefabs.Where(x => x.name == attackName).First().prefab);
        return wasReady;
    }

    public void FacingLeftOrRight(bool facingRight)
    {
        this.facingRight = facingRight;
        billboard.flipX = billboardFacingRight ? !facingRight : facingRight;
    }

    private void AttackCoroutine(Transform target, GameObject attackPrefab)
    {
        if (readyToAttack && notBusy)
        {
            Quaternion newRotation = transform.rotation;
            if (!facingRight)
            {
                newRotation *= Quaternion.Euler(0, 180, 0);
            }
            GameObject projectile = Instantiate(attackPrefab, transform.position, newRotation);
            if(target != null && projectile.TryGetComponent(out TargetedProjectile targeting))
            {
                targeting.target = target;
            }
            for (int i = 0; i < projectile.transform.childCount; i++)
            {
                if (projectile.transform.GetChild(i).TryGetComponent(out Hitbox hitbox))
                {
                    if (hitbox.healthEffect < 0)
                    {
                        hitbox.healthEffect *= damageMultiplier;
                    }
                }
            }
        }
    }

    [Serializable]
    private struct NamedAttack
    {
        public string name;
        public GameObject prefab;
    }
}

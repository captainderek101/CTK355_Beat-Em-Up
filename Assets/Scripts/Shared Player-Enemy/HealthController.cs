using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthController : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private bool isEnemy = false;

    private const string playerTagName = "Player";
    private const string enemyTagName = "Enemy";
    private const string playerHitboxTagName = "Player Hitbox";
    private const string enemyHitboxTagName = "Enemy Hitbox";

    private Health health;

    public DeathEvent deathEvents;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.death += () =>
        {
            deathEvents.Invoke();
        };
        if (gameObject.tag == playerTagName)
        {
            isPlayer = true;
        }
        else if(gameObject.tag == enemyTagName)
        {
            isEnemy = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox otherHitbox;
        if(isPlayer)
        {
            if (other.tag == enemyHitboxTagName && other.TryGetComponent<Hitbox>(out otherHitbox))
            {
                health.ChangeHealth(otherHitbox.healthEffect);
                Destroy(other.gameObject);
            }
        }
        else if(isEnemy)
        {
            if (other.tag == playerHitboxTagName && other.TryGetComponent<Hitbox>(out otherHitbox))
            {
                health.ChangeHealth(otherHitbox.healthEffect);
                Destroy(other);
            }
        }
    }
}

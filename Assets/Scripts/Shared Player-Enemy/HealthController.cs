using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class HealthController : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private bool isEnemy = false;

    private const string playerTagName = "Player";
    private const string enemyTagName = "Enemy";
    private const string playerHitboxTagName = "Player Hitbox";
    private const string enemyHitboxTagName = "Enemy Hitbox";
    private const string hurtAudioName = "hurt";
    private const string hurtAnimationTrigger = "Hit";
    private const string deadAnimationTrigger = "Dead";

    private Health health;
    private AudioPlayer audioPlayer;
    private Animator animator;

    public UnityEvent deathEvents;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.death += () =>
        {
            if(deathEvents != null)
            {
                deathEvents.Invoke();
            }
        };
        Animator animator;
        if (TryGetComponent(out animator))
        {
            deathEvents.AddListener(() => animator.SetTrigger(deadAnimationTrigger));
        }
        if (gameObject.tag == playerTagName)
        {
            isPlayer = true;
        }
        else if(gameObject.tag == enemyTagName)
        {
            isEnemy = true;
        }
    }

    public void ChangeHealth(float amount)
    {
        health.ChangeHealth(amount);
        if (audioPlayer == null)
        {
            TryGetComponent(out audioPlayer);
        }
        audioPlayer.PlaySound(hurtAudioName);
        if (animator == null)
        {
            TryGetComponent(out animator);
        }
        animator.SetTrigger(hurtAnimationTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox otherHitbox;
        if (health.dead)
        {
            return;
        }
        if (isPlayer)
        {
            if (other.tag == enemyHitboxTagName && other.TryGetComponent<Hitbox>(out otherHitbox))
            {
                ChangeHealth(otherHitbox.healthEffect);
                Destroy(other.gameObject);
            }
        }
        else if(isEnemy)
        {
            if (other.tag == playerHitboxTagName && other.TryGetComponent<Hitbox>(out otherHitbox))
            {
                ChangeHealth(otherHitbox.healthEffect);
                Destroy(other);
            }
        }
    }
}

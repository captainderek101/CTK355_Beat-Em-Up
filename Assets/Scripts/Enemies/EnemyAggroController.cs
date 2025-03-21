using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(HealthController))]
public class EnemyAggroController : MonoBehaviour
{
    [SerializeField] private float maxAggroDistance = 2f;
    [SerializeField] private float minDeaggroDistance = 3f;
    public bool aggroed = false;
    [SerializeField] private MonoBehaviour movementComponent;
    private Wander wanderComponent;
    private Transform playerTransform;
    private HealthController healthController;

    private void Start()
    {
        healthController = GetComponent<HealthController>();
        healthController.deathEvents.AddListener(() => EnemyManager.Instance.DeaggroEnemy());
        if(transform.parent.TryGetComponent(out ScreenManager screenManager)) // are we part of a screen?
        {
            healthController.deathEvents.AddListener(() => screenManager.SpawnDefeated());
        }
        TryGetComponent(out wanderComponent);
        if(movementComponent == null)
        {
            Debug.LogWarning(gameObject.name + " " + nameof(EnemyAggroController) + " is missing its movement component! This will cause errors!");
        }
        else
        {
            movementComponent.enabled = aggroed;
            if(wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
        }
        if (GameManager.Instance.playerObjects != null)
        {
            playerTransform = GameManager.Instance.playerObjects[Random.Range(0, GameManager.Instance.playerObjects.Length)].transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
        {
            aggroed = false;
            movementComponent.enabled = aggroed;
            if (wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
            return;
        }
        if (!aggroed && (playerTransform.position - transform.position).magnitude < maxAggroDistance)
        {
            aggroed = EnemyManager.Instance.TryAggroEnemy();
            movementComponent.enabled = aggroed;
            if (wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
        }
        else if (aggroed && (playerTransform.position - transform.position).magnitude > minDeaggroDistance)
        {
            EnemyManager.Instance.DeaggroEnemy();
            aggroed = false;
            movementComponent.enabled = aggroed;
            if (wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
        }
    }

    public void RecordDeath()
    {
        EnemyManager.Instance.RecordEnemyDeath();
    }

    private void OnBecameVisible()
    {
        // Only allow for aggroing once visible?
    }
}

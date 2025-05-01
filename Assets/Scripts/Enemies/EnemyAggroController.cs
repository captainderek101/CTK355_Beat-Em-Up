using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(HealthController))]
public class EnemyAggroController : MonoBehaviour
{
    [SerializeField] private float maxAggroDistance = 2f;
    [SerializeField] private float minDeaggroDistance = 3f;
    public bool aggroed = false;
    [SerializeField] private MoveTowardsPlayer movementComponent;
    [SerializeField] private EnemyAttackController attackController;
    [SerializeField] private EnemyDirectionController directionController;
    private Wander wanderComponent;
    private Transform playerTransform;
    private HealthController healthController;

    private void Start()
    {
        healthController = GetComponent<HealthController>();
        healthController.deathEvents.AddListener((GameObject source) => EnemyManager.Instance.DeaggroEnemy());
        if(transform.parent.TryGetComponent(out ScreenManager screenManager)) // are we part of a screen?
        {
            healthController.deathEvents.AddListener((GameObject source) => screenManager.SpawnDefeated());
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
            FindNewPlayerTarget();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || playerTransform.gameObject.activeSelf == false)
        {
            aggroed = false;
            movementComponent.enabled = aggroed;
            if (wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
            Debug.Log(gameObject.name + " choosing new player target");
            FindNewPlayerTarget();
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

    public void RecordDeath(GameObject source)
    {
        EnemyManager.Instance.RecordEnemyDeath(source);
    }

    private void OnBecameVisible()
    {
        FindNewPlayerTarget();
    }

    private void FindNewPlayerTarget()
    {
        IEnumerable<GameObject> activePlayers = GameManager.Instance.playerObjects.Where(x => x.activeSelf);
        if (activePlayers.Any())
        {
            playerTransform = activePlayers.ElementAt(Random.Range(0, activePlayers.Count())).transform;
            attackController.playerTransform = playerTransform;
            movementComponent.playerTransform = playerTransform;
            directionController.playerTransform = playerTransform;
        }
    }
}

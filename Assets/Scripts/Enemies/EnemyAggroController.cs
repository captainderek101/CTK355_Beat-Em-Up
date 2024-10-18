using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroController : MonoBehaviour
{
    [SerializeField] private float maxAggroDistance = 2f;
    [SerializeField] private float minDeaggroDistance = 3f;
    public bool aggroed = false;
    [SerializeField] private MonoBehaviour movementComponent;
    private Wander wanderComponent;
    private Transform playerTransform;

    private void Start()
    {
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
        if (GameManager.Instance.playerObject != null)
        {
            playerTransform = GameManager.Instance.playerObject.transform;
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
            aggroed = !EnemyManager.Instance.DeaggroEnemy();
            movementComponent.enabled = aggroed;
            if (wanderComponent != null)
            {
                wanderComponent.enabled = !aggroed;
            }
        }
    }

    private void OnBecameVisible()
    {
        // Only allow for aggroing once visible?
    }
}

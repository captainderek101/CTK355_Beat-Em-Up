using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject enemy;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning("enemy spawner has no parent transform!");
            enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        }
        else
        {
            enemy = Instantiate(enemyPrefab, transform.position, transform.rotation, transform.parent);
        }
    }
}

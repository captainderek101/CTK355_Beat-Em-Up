using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth = 5;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private float currentHealth;
    Vector3 checkpointPos;

    private void Start()
    {
        currentHealth = initialHealth;
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth < 0.01f)
        {
            Die();
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died!");
        transform.position = checkpointPos;
    }
    public void UpdateCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    }
}

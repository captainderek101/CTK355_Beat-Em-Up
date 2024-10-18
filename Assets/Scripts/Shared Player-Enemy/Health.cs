using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathEvent();
public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth = 5;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private float currentHealth;
    public bool destroyOnDeath = true;

    public DeathEvent death;

    private void Awake()
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        if(death != null)
        {
            death();
        }
        //Debug.Log(gameObject.name + " died!");
        if(destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}

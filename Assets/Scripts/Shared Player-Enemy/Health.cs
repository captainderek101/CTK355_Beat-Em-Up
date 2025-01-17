using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathEvent();
public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth = 5;
    public float maxHealth = 5;
    [SerializeField] private float currentHealth;
    public bool destroyOnDeath = true;

    public DeathEvent death;
    [HideInInspector] public bool dead = false;

    public GameObject[] itemDrops;

    private void Awake()
    {
        currentHealth = initialHealth;
    }

    public void ChangeHealth(float amount)
    {
        if (dead) // we already died
        {
            return;
        }
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

    private void Die()
    {
        if(death != null)
        {
            death();
        }
        //Debug.Log(gameObject.name + " died!");
        if(destroyOnDeath)
        {
            //Destroy(gameObject);
            ItemDrop();
        }
        dead = true;
    }
    private void ItemDrop()
    {
        for (int i = 0; i < itemDrops.Length; i++)
        {
            Instantiate(itemDrops[i], transform.position, Quaternion.identity);
        }
    }
}

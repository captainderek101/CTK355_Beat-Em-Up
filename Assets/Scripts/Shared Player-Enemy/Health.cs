using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void DeathEvent(GameObject source);
public class Health : MonoBehaviour
{
    [SerializeField] private float initialHealth = 5;
    public float maxHealth = 5;
    [SerializeField] private float currentHealth;
    public bool destroyOnDeath = true;

    public DeathEvent death;
    [HideInInspector] public bool dead = false;

    public GameObject[] itemDrops;
    private EntityUIManager entityUI;

    private void Awake()
    {
        currentHealth = initialHealth;
        TryGetComponent(out entityUI);
    }

    public void ChangeHealth(float amount, GameObject source = null)
    {
        if (dead) // we already died
        {
            return;
        }
        currentHealth += amount;
        if (currentHealth < 0.01f)
        {
            currentHealth = 0;
            Die(source);
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (entityUI != null)
        {
            entityUI.SetHealthBarUI(currentHealth / maxHealth);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die(GameObject source)
    {
        if(death != null)
        {
            death(source);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;
    public float baseHorizontalMoveSpeed;
    public float baseVerticalMoveSpeed;
    public float currentMoveSpeedMultiplier;
    public float baseMaxHealth;
    public float currentMaxHealthMultiplier;
    public float currentDamageMultiplier;
    public int currentAbilityChargeLimit;

    private const int baseAbilityChargeLimit = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public void ApplyPlayerStats(GameObject player)
    {
        ApplyMovementEffects(player);
        ApplyHealthEffects(player);
        ApplyAttackEffects(player);
    }
    public void ResetPlayerStats()
    {
        currentMoveSpeedMultiplier = 1;
        currentMaxHealthMultiplier = 1;
        currentDamageMultiplier = 1;
        currentAbilityChargeLimit = baseAbilityChargeLimit;

        foreach (GameObject playerObject in GameManager.Instance.playerObjects)
        {
            ApplyPlayerStats(playerObject);
        }
    }
    private void ApplyMovementEffects(GameObject player)
    {
        var movementController = player.GetComponent<PlayerMovementController>();
        movementController.horizontalMoveSpeed = baseHorizontalMoveSpeed * currentMoveSpeedMultiplier;
        movementController.verticalMoveSpeed = baseVerticalMoveSpeed * currentMoveSpeedMultiplier;
    }
    private void ApplyHealthEffects(GameObject player)
    {
        var health = player.GetComponent<Health>();
        float initialMaxHealth = health.maxHealth;
        health.maxHealth = baseMaxHealth * currentMaxHealthMultiplier;
        if(health.maxHealth - baseMaxHealth > 0.01f)
            health.ChangeHealth(health.maxHealth - baseMaxHealth);
    }
    private void ApplyAttackEffects(GameObject player)
    {
        var attackController = player.GetComponent<PlayerAttackController>();
        attackController.damageMultiplier = currentDamageMultiplier;
        attackController.SetAbilityChargeLimit(currentAbilityChargeLimit);
    }
}

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
        health.ChangeHealth(health.maxHealth - initialMaxHealth);
    }
    private void ApplyAttackEffects(GameObject player)
    {
        var attackController = player.GetComponent<PlayerAttackController>();
        attackController.damageMultiplier = currentDamageMultiplier;
        attackController.SetAbilityChargeLimit(currentAbilityChargeLimit);
    }
}

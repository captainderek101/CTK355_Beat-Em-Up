using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityUIManager : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private UIGroupControl abilityUI;
    private Slider healthBarSlider;

    private void Awake()
    {
        if(healthBar != null)
        {
            healthBar.TryGetComponent(out healthBarSlider);
        }
    }

    public void ShowHealthBar(bool show)
    {
        healthBar.SetActive(show);
    }

    public void SetHealthBarUI(float percentHealth)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = percentHealth;
        }
    }

    public void ShowAbilityCharge(bool show)
    {
        abilityUI.gameObject.SetActive(show);
    }

    public void UpdateAbilityUI(int currentAbilityCharge, int abilityChargeLimit)
    {
        abilityUI.SetSliderValue(Mathf.Clamp((float)currentAbilityCharge / abilityChargeLimit, 0, 1));
        abilityUI.SetTextValue((int)Mathf.Min(currentAbilityCharge, abilityChargeLimit) + " / " + abilityChargeLimit);
    }
}

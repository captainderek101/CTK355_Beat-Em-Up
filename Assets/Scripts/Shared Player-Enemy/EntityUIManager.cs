using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityUIManager : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
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
}

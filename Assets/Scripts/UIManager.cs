using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component controls how the Water level of the player is displayed
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Slider playerHealthSlider;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if(playerHealthSlider == null)
        {
            Debug.LogWarning(nameof(UIManager) + " is missing player health slider!");
        }
    }

    public void SetPlayerHealthUI(float percentWaterLevel)
    {
        playerHealthSlider.value = percentWaterLevel;
    }
}

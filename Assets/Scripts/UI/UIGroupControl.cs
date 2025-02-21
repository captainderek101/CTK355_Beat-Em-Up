using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGroupControl : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text displayText;

    public void SetSliderValue(float amount)
    {
        slider.value = amount;
    }

    public void SetTextValue(string text)
    {
        displayText.text = text;
    }
}

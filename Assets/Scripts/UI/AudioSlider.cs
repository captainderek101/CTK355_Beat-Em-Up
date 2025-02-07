using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private AudioMixMode mixMode;
    [SerializeField]
    private string parameterName = "REPLACE THIS";

    private void Start()
    {
        slider.onValueChanged.AddListener( (e) => OnChangeSlider(e) );
        slider.value = PlayerPrefs.GetFloat(parameterName, 1);
        switch (mixMode)
        {
            case AudioMixMode.LinearMixerVolume:
                mixer.SetFloat(parameterName, -80 + (PlayerPrefs.GetFloat(parameterName, 1) * 80));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                mixer.SetFloat(parameterName, Mathf.Log10(PlayerPrefs.GetFloat(parameterName, 1)) * 20);
                break;
        }
    }

    public void OnChangeSlider(float value)
    {
        if (value < 0.001f)
        {
            mixer.SetFloat(parameterName, -80f);
        }
        else
        {
            switch (mixMode)
            {
                case AudioMixMode.LinearMixerVolume:
                    mixer.SetFloat(parameterName, -80 + (value * 80));
                    break;
                case AudioMixMode.LogrithmicMixerVolume:
                    mixer.SetFloat(parameterName, Mathf.Log10(value) * 20);
                    break;
            }
        }

        PlayerPrefs.SetFloat(parameterName, value);
        PlayerPrefs.Save();
    }

    public enum AudioMixMode
    {
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}
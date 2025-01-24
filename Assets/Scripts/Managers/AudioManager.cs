using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string exposedVolumeParameter = "Volume";
    [SerializeField] private AnimationCurve volumeChangeCurve;

    private const float baseVolume_dB = 0; // TODO: don't use this!! get the volume that was set beforehand!! store in playerprefs maybe?

    private void Start()
    {
        if(mixer == null)
        {
            Debug.LogWarning("Audio Manager on " + gameObject.name + " - Audio Mixer is not set!!");
        }
        TransitionManager.Instance.transitionEvent += (float duration) => { SetVolumeSteady(duration); };
        if (GameManager.Instance.inFirstLoadedScene == false)
        {
            SetVolumeSteady(TransitionManager.Instance.defaultTransitionTime, baseVolume_dB); // TODO
        }
    }

    private void SetVolumeSteady(float transitionTime, float volume_dB = -80)
    {
        StartCoroutine(ChangeVolumeCoroutine(transitionTime, volume_dB));
    }

    private IEnumerator ChangeVolumeCoroutine(float transitionTime, float volume_dB)
    {
        float timeSinceStart = 0;
        float current;
        mixer.GetFloat(exposedVolumeParameter, out float startingVolume);
        float startingVolumeAboveTarget = startingVolume - volume_dB;
        while (timeSinceStart < transitionTime)
        {
            current = volumeChangeCurve.Evaluate(timeSinceStart / transitionTime);
            yield return new WaitForEndOfFrame();
            mixer.SetFloat(exposedVolumeParameter, startingVolume - (startingVolumeAboveTarget * current));
            timeSinceStart += Time.deltaTime;
        }
    }
}

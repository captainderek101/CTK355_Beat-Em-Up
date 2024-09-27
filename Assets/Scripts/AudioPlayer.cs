using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [Serializable]
    public struct AudioEvent
    {
        public string name;
        public AudioClip[] clips;
        [Range(0.1f, 2.0f)]
        public float pitch;
        [Range(0.1f, 2.0f)]
        public float volume;
        [Tooltip("Add +/- 0.1f to pitch and volume when played.")]
        public bool randomizePitchAndVolume; //TODO: make this actually do something!!
    }
    public AudioEvent[] events;
    [SerializeField] private AudioSource source;

    public void PlaySound(string name)
    {
        if (events.Where(x => x.name == name).Count() == 0)
        {
            Debug.LogWarning("AudioPlayer on " + gameObject.name + " tried to play sound that doesn't exist: " + name);
            return;
        }
        else if (events.Where(x => x.name == name).Count() > 1)
        {
            Debug.LogWarning("AudioPlayer on " + gameObject.name + " has multiple sounds with name: " + name);
        }

        AudioEvent thisEvent = events.Where(x => x.name == name).First();
        AudioClip thisClip = thisEvent.clips[UnityEngine.Random.Range(0, thisEvent.clips.Length - 1)];
        source.pitch = thisEvent.pitch;
        source.volume = thisEvent.volume;
        source.PlayOneShot(thisClip);
    }
}

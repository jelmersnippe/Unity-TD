using System;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private float volume;

    public List<Sound> sounds = new List<Sound>();
    List<Sound.Name> usedSoundNames = new List<Sound.Name>();

    private void OnEnable()
    {
        UIController.OnVolumeChanged += UpdateVolume;
    }

    private void OnDisable()
    {
        UIController.OnVolumeChanged -= UpdateVolume;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < sounds.Count; i++)
        {
            Sound sound = sounds[i];

            if (usedSoundNames.Contains(sound.name))
            {
                Debug.LogError("Two sounds are being registered with the name " + sound.name + ". Duplicate usage at index " + i.ToString());
            }

            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * volume;
            sound.source.pitch = sound.pitch;

            usedSoundNames.Add(sound.name);
        }
    }

    public void Play(Sound.Name name)
    {
        Sound existingSound = sounds.Find((sound) => sound.name == name);

        if (existingSound == null)
        {
            Debug.LogError("Tried to play a non existing sound called " + name);
            return;
        }

        existingSound.source.Play();
    }

    void UpdateVolume(float newVolume)
    {
        volume = newVolume;
        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume * volume;
        }
    }
}

[System.Serializable]
public class Sound
{
    public enum Name
    {
        Escape,
        Explosion,
        Shoot,
        WaveChange,
        Shotgun
    }

    public Name name;
    public AudioClip clip;

    [Min(0f)]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Min(0.1f)]
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}
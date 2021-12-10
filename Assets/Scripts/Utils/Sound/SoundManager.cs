using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

using ThePattern.Unity;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<Sound> _sfxSounds;
    [SerializeField] private List<Sound> _bgmSounds;

    protected void Awake()
    {
        _sfxSounds.ForEach(sound =>
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.loop = false;
        });
        _bgmSounds.ForEach(sound =>
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.loop = true;
        });
        StorageUserInfo.Instance.PlayerData.IsMusicOn.RegisterNotifyOnChanged(value =>
        {
            _bgmSounds.ForEach(sound => sound.audioSource.mute = !value);
        });
        StorageUserInfo.Instance.PlayerData.IsSoundOn.RegisterNotifyOnChanged(value =>
        {
            _sfxSounds.ForEach(sound => sound.audioSource.mute = !value);
        });
        _bgmSounds.ForEach(sound => sound.audioSource.mute = !StorageUserInfo.Instance.PlayerData.IsMusicOn.Value);
        _sfxSounds.ForEach(sound => sound.audioSource.mute = !StorageUserInfo.Instance.PlayerData.IsSoundOn.Value);
    }

    public void PlaySFX(string name)
    {
        Sound sound = _sfxSounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogError($"SFX: {sound.name} not found");
            return;
        }
        sound.audioSource.Play();
    }

    public void PlayBGM(string name)
    {
        Sound sound = _bgmSounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogError($"BGM: {sound.name} not found");
            return;
        }
        sound.audioSource.Play();
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector]
    public AudioSource audioSource;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;
    [Header("AudioSource")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("UI Slider")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("BGM Audio Clip")]
    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip inGameBGM;

    [Header("SFX Audio Clip")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip skillSfx;


    private void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        masterSlider.value = DataManager.instance.soundData.master;
        bgmSlider.value = DataManager.instance.soundData.bgm;
        sfxSlider.value = DataManager.instance.soundData.sfx;
        PlayMusic(0);
    }


    private void SetMasterVolume(float value)
    {
        DataManager.instance.soundData.master = value;
        float volume = Mathf.Lerp(-80f, 0f, value / 9f);
        audioMixer.SetFloat("Master", volume);
    }
    private void SetBGMVolume(float value)
    {
        DataManager.instance.soundData.bgm = value;
        float volume = Mathf.Lerp(-80f, 0f, value / 9f);
        audioMixer.SetFloat("BGM", volume);
    }
    private void SetSFXVolume(float value)
    {
        DataManager.instance.soundData.sfx = value;
        float volume = Mathf.Lerp(-80f, 0f, value / 9f);
        audioMixer.SetFloat("SFX", volume);
    }

    public void PlayMusic(int index)
    {
        switch (index)
        {
            case 0:
                bgmAudioSource.clip = mainMenuBGM;
                break;
            case 1:
                bgmAudioSource.clip = inGameBGM;
                break;
        }
        bgmAudioSource.Play();
    }

    public void PlaySFX(int index)
    { 
        switch(index)
        {
            case 0:
                sfxAudioSource.PlayOneShot(buttonClick);
                break;
            case 1:
                sfxAudioSource.PlayOneShot(deathSfx);
                break;
            case 2:
                sfxAudioSource.PlayOneShot(jumpSfx);
                break;
            case 3:
                sfxAudioSource.PlayOneShot(skillSfx);
                break;

        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
	private enum eAudioType
    {
		Master = 0,
		BGM,
		SFX
    }

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private eAudioType audioType;

	private Slider audioSlider;

    private void Awake()
    {
        TryGetComponent(out audioSlider);
    }

    private void OnEnable()
    {
        switch (audioType)
        {
            case eAudioType.Master:
                audioSlider.value = AudioManager.Instance.master;
                audioSlider.onValueChanged.AddListener(SetMasterVolume);
                break;
            case eAudioType.BGM:
                audioSlider.value = AudioManager.Instance.BGM;
                audioSlider.onValueChanged.AddListener(SetBGMVolume);
                break;
            case eAudioType.SFX:
                audioSlider.value = AudioManager.Instance.SFX;
                audioSlider.onValueChanged.AddListener(SetSFXVolume);
                break;
        }
    }

    private void OnDisable()
    {
        switch (audioType)
        {
            case eAudioType.Master:
                audioSlider.value = AudioManager.Instance.master;
                audioSlider.onValueChanged.RemoveListener(SetMasterVolume);
                break;
            case eAudioType.BGM:
                audioSlider.value = AudioManager.Instance.BGM;
                audioSlider.onValueChanged.RemoveListener(SetBGMVolume);
                break;
            case eAudioType.SFX:
                audioSlider.value = AudioManager.Instance.SFX;
                audioSlider.onValueChanged.RemoveListener(SetSFXVolume);
                break;
        }
    }


    private void SetMasterVolume(float volume)
    {
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("Master", dBValue);

        
            AudioManager.Instance.master = volume;
    }

    private void SetBGMVolume(float volume)
    {
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("BGM", dBValue);
        
            AudioManager.Instance.BGM = volume;
    }

    private void SetSFXVolume(float volume)
    {
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("SFX", dBValue);
        
            AudioManager.Instance.SFX = volume;
    }
    
}
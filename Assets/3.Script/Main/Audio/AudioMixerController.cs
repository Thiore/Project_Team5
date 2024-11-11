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

    private void Start()
    {
        TryGetComponent(out audioSlider);
        switch (audioType)
        {
            case eAudioType.Master:
                audioSlider.onValueChanged.AddListener(SetMasterVolume);
                break;
            case eAudioType.BGM:
                audioSlider.onValueChanged.AddListener(SetBGMVolume);
                break;
            case eAudioType.SFX:
                audioSlider.onValueChanged.AddListener(SetSFXVolume);
                break;
        }
    }

    private void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20f);

        if (audioMixer.GetFloat("Master", out float value))
            AudioManager.Instance.master = value;
    }

    private void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        if (audioMixer.GetFloat("BGM", out float value))
            AudioManager.Instance.BGM = value;
    }

    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
        if (audioMixer.GetFloat("SFX", out float value))
            AudioManager.Instance.SFX = value;
    }
    
}
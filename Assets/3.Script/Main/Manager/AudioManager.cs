using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField] private AudioMixer m_AudioMixer;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        //m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        //m_MusicSESlider.onValueChanged.AddListener(SetSFXVolume);
    }
}

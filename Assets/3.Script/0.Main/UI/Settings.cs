using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private enum eSlideType
    {
        Default = 0,
        Master,
        BGM,
        SFX,
        CameraSpeed
    }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private eSlideType slideType;


    private Slider typeSlider;

    [SerializeField] private GameObject introBtn;

    private void Awake()
    {
        TryGetComponent(out typeSlider);
    }

    private void OnEnable()
    {
        switch (slideType)
        {
            case eSlideType.Default:
                if (SceneManager.GetSceneByName("Lobby").Equals(SceneManager.GetActiveScene()))
                {
                    introBtn.SetActive(false);
                }
                else
                    introBtn.SetActive(true);

                SceneManager.sceneLoaded += OnSettingLoaded;
                break;
            case eSlideType.Master:
                typeSlider.onValueChanged.AddListener(SetMasterVolume);
                typeSlider.value = SettingsManager.Instance.master;
                break;
            case eSlideType.BGM:
                typeSlider.onValueChanged.AddListener(SetBGMVolume);
                typeSlider.value = SettingsManager.Instance.BGM;
                break;
            case eSlideType.SFX:
                typeSlider.onValueChanged.AddListener(SetSFXVolume);
                typeSlider.value = SettingsManager.Instance.SFX;
                break;
            case eSlideType.CameraSpeed:
                typeSlider.value = SettingsManager.Instance.CameraSpeed;
                break;
        }
        

    }

    private void OnDisable()
    {
        switch (slideType)
        {
            case eSlideType.Default:
                SceneManager.sceneLoaded -= OnSettingLoaded;
                break;
            case eSlideType.Master:
                SettingsManager.Instance.master = typeSlider.value;
                typeSlider.onValueChanged.RemoveListener(SetMasterVolume);
                PlayerPrefs.SetFloat(SettingsManager.Instance.hashMaster, typeSlider.value);
                break;
            case eSlideType.BGM:
                SettingsManager.Instance.BGM = typeSlider.value;
                PlayerPrefs.SetFloat(SettingsManager.Instance.hashBGM, typeSlider.value);
                typeSlider.onValueChanged.RemoveListener(SetBGMVolume);
                break;
            case eSlideType.SFX:
                SettingsManager.Instance.SFX = typeSlider.value;
                typeSlider.onValueChanged.RemoveListener(SetSFXVolume);
                PlayerPrefs.SetFloat(SettingsManager.Instance.hashSFX, typeSlider.value);
                break;
            case eSlideType.CameraSpeed:
                SettingsManager.Instance.CameraSpeed = typeSlider.value;
                PlayerPrefs.SetFloat(SettingsManager.Instance.hashCameraSpeed, typeSlider.value);
                break;
        }
    }

    

    private void OnSettingLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
    public void LoadLobby()
    {
        GameManager.Instance.LoadScene("Lobby");
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void SetMasterVolume(float volume)
    {
        float dBValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f; // 0 ~ 1 -> -80dB ~ 0dB
        audioMixer.SetFloat("Master", dBValue);

    }

    private void SetBGMVolume(float volume)
    {
        float dBValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f; // 0 ~ 1 -> -80dB ~ 0dB
        audioMixer.SetFloat("BGM", dBValue);

    }

    private void SetSFXVolume(float volume)
    {
        float dBValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f; // 0 ~ 1 -> -80dB ~ 0dB
        audioMixer.SetFloat("SFX", dBValue);

    }

    

}

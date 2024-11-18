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
                typeSlider.value = SettingsManager.Instance.master;
                typeSlider.onValueChanged.AddListener(SetMasterVolume);
                break;
            case eSlideType.BGM:
                typeSlider.value = SettingsManager.Instance.BGM;
                typeSlider.onValueChanged.AddListener(SetBGMVolume);
                break;
            case eSlideType.SFX:
                typeSlider.value = SettingsManager.Instance.SFX;
                typeSlider.onValueChanged.AddListener(SetSFXVolume);
                break;
            case eSlideType.CameraSpeed:
                typeSlider.value = SettingsManager.Instance.CameraSpeed;
                typeSlider.onValueChanged.AddListener(SetCamSpeed);
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
                break;
            case eSlideType.BGM:
                SettingsManager.Instance.BGM = typeSlider.value;
                typeSlider.onValueChanged.RemoveListener(SetBGMVolume);
                break;
            case eSlideType.SFX:
                SettingsManager.Instance.SFX = typeSlider.value;
                typeSlider.onValueChanged.RemoveListener(SetSFXVolume);
                break;
            case eSlideType.CameraSpeed:
                SettingsManager.Instance.CameraSpeed = typeSlider.value;
                typeSlider.onValueChanged.RemoveListener(SetCamSpeed);
                break;
        }
    }
    private void OnSettingLoaded(Scene scene, LoadSceneMode mode)
    {
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.optionBtn.onClick.AddListener(OnSettingPage);
        }
    }
    public void LoadLobby()
    {
        SaveManager.Instance.SaveGameState();
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
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("Master", dBValue);


        SettingsManager.Instance.master = volume;
    }

    private void SetBGMVolume(float volume)
    {
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("BGM", dBValue);

        SettingsManager.Instance.BGM = volume;
    }

    private void SetSFXVolume(float volume)
    {
        float dBValue = Mathf.Lerp(-60f, 0f, volume);
        audioMixer.SetFloat("SFX", dBValue);

        SettingsManager.Instance.SFX = volume;
    }

    private void SetCamSpeed(float speed)
    {
        float speedValue = Mathf.Lerp(0.1f, 5f, speed);

        SettingsManager.Instance.CameraSpeed = speedValue;
    }
    public void SaveComplete()
    {
        //SaveManager.Instance.Update~~ 4가지 값저장
        SaveManager.Instance.UpdateVolumeSettings(
            SettingsManager.Instance.master,
            SettingsManager.Instance.BGM,
            SettingsManager.Instance.SFX,
            SettingsManager.Instance.CameraSpeed);
        Debug.Log("SaveComplete");
        
    }

    private void OnSettingPage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}

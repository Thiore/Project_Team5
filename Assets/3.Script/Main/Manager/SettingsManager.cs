using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; } = null;

    [SerializeField] private AudioMixer audioMixer;
    [HideInInspector]
    public float master;
    [HideInInspector]
    public float BGM;
    [HideInInspector]
    public float SFX;
    [HideInInspector]
    public float CameraSpeed;

    [SerializeField] private GameObject settingPage;

    public TMP_Text koreanButtonText;//한국어 버튼

    public TMP_Text englishButtonText;//한국어 버튼

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
        //승주씨 수정부탁드려요? + Settings SaveComplete부분에 저장하는거 넣어주시면됩니다.
        //setting창 끄는버튼에 저 메서드들 추가해뒀어요~
        master = SaveManager.Instance.gameState.masterVolume;
        BGM = SaveManager.Instance.gameState.bgmVoluem;
        SFX = SaveManager.Instance.gameState.sfxVoluem;
        CameraSpeed = SaveManager.Instance.gameState.camSpeed;

        float masterValue = Mathf.Lerp(-60f, 0f, master);
        audioMixer.SetFloat("Master", masterValue);

        float BGMValue = Mathf.Lerp(-60f, 0f, BGM);
        audioMixer.SetFloat("BGM", BGMValue);
       
        float SFXValue = Mathf.Lerp(-60f, 0f, SFX);
        audioMixer.SetFloat("SFX", SFXValue);
    }

    public void OnSettingPage()
    {
        settingPage.SetActive(true);
    }
}

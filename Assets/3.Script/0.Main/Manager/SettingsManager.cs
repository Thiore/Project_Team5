using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; } = null;


    [SerializeField] private AudioMixer audioMixer;

    public readonly string hashMaster = "Master";  
    [HideInInspector]
    public float master;

    public readonly string hashBGM = "BGM";
    [HideInInspector]
    public float BGM;

    public readonly string hashSFX = "SFX";
    [HideInInspector]
    public float SFX;

    public readonly string hashCameraSpeed = "CameraSpeed";
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

            InitSetting();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        float masterValue = Mathf.Log10(Mathf.Clamp(master, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Master", masterValue);

        float BGMValue = Mathf.Log10(Mathf.Clamp(BGM, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("BGM", BGMValue);
       
        float SFXValue = Mathf.Log10(Mathf.Clamp(SFX, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFX", SFXValue);
    }

    public void OnSettingPage()
    {
        settingPage.SetActive(true);
    }
    private void InitSetting()
    {
        if (PlayerPrefs.HasKey(hashMaster))
        {
            master = PlayerPrefs.GetFloat(hashMaster);
        }
        else
            master = 0.5f;

        if (PlayerPrefs.HasKey(hashBGM))
        {
            BGM = PlayerPrefs.GetFloat(hashBGM);
        }
        else
            BGM = 0.5f;

        if (PlayerPrefs.HasKey(hashSFX))
        {
            SFX = PlayerPrefs.GetFloat(hashSFX);
        }
        else
            SFX = 0.5f;

        if (PlayerPrefs.HasKey(hashCameraSpeed))
        {
            CameraSpeed = PlayerPrefs.GetFloat(hashCameraSpeed);
        }
        else
            CameraSpeed = 0.5f;

       
    }

   
}

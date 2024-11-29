using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

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

    [SerializeField] private Toggle koreanText;//한국어 버튼

    [SerializeField] private Toggle englishText;//한국어 버튼

    [SerializeField] private GameObject introBtn;

    private Color activeColor = Color.green; //활성화 중인 언어 텍스트의 색상
    private Color inactiveColor = Color.white; //비활성화 중인 언어 텍스트의 색상


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            InitSetting();
            SceneManager.sceneLoaded += LoadedScene;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= LoadedScene;
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

    private void LoadedScene(Scene scene, LoadSceneMode mode)
    {
        englishText.onValueChanged.AddListener(delegate { ChangeLocale(); });
        koreanText.onValueChanged.AddListener(delegate { ChangeLocale(); });

        if (DialogueManager.Instance.selectedLocale.Equals("ko"))
        {
            koreanText.isOn = true;
        }
        else
        {
            englishText.isOn = true;
        }
        ChangeLocale();
        if(!scene.name.Equals("Lobby"))
        {
            introBtn.SetActive(true);
        }
        else
        {
            introBtn.SetActive(false);
        }
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

    public void ChangeLocale()
    {
        if(koreanText.isOn)
        {
            DialogueManager.Instance.ChangeLocale(1);
            koreanText.targetGraphic.color = activeColor;
            englishText.targetGraphic.color = inactiveColor;
        }
        if(englishText.isOn)
        {
            DialogueManager.Instance.ChangeLocale(0);
            englishText.targetGraphic.color = activeColor;
            koreanText.targetGraphic.color = inactiveColor;
        }
        

    }

    public void LoadLobby()
    {
        GameManager.Instance.LoadLobby();
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField] private AudioMixer audioMixer;
    [HideInInspector]
    public float master;
    [HideInInspector]
    public float BGM;
    [HideInInspector]
    public float SFX;


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

        master = 0.5f;
        BGM = 0.5f;
        SFX = 0.5f;

        float masterValue = Mathf.Lerp(-60f, 0f, master);
        audioMixer.SetFloat("Master", masterValue);

        float BGMValue = Mathf.Lerp(-60f, 0f, BGM);
        audioMixer.SetFloat("BGM", BGMValue);
       
        float SFXValue = Mathf.Lerp(-60f, 0f, SFX);
        audioMixer.SetFloat("SFX", SFXValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } = null;

    [SerializeField] private AudioMixer audioMixer;

    public float master;
    public float BGM;
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
        audioMixer.GetFloat("Master", out float masterValue);
        master = masterValue;

        audioMixer.GetFloat("BGM", out float BGMValue);
        BGM = BGMValue;

        audioMixer.GetFloat("SFX", out float SFXValue);
        SFX = SFXValue;
    }
}

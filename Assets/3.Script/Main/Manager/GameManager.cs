using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManager instance = null;
    public static GameManager Instance { get; private set; }
    [HideInInspector]
    public bool isInput;

    [SerializeField] private Toggle lanternObj;
    [SerializeField] private GameObject lanternLight;

    //임시
    
    [SerializeField] private bool isEmptyLantern;
    [SerializeField] private bool isBattery;
    [SerializeField] public bool isLantern { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Instance = instance;
            
        }
        //    if (instance == null)
        //    {
        //        instance = this;
        //        Instance = instance;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        }
        //임시
        private void Start()
    {
        isEmptyLantern = false;
        isBattery = false;
        isLantern = false;

    }

    public void GetBattery()
    {
        isBattery = true;
        if(isBattery && isEmptyLantern)
        {
            isLantern = true;
            SetLantern();
        }
    }
    public void GetEmptyLantern()
    {
        isEmptyLantern = true;
        if (isBattery && isEmptyLantern)
        {
            isLantern = true;
            SetLantern();
        }
    }
    private void SetLantern()
    {
        lanternObj.gameObject.SetActive(true);
    }
    public void OnLantern()
    {
        if(lanternObj.isOn)
        {
            lanternLight.SetActive(true);
        }
        else
        {
            lanternLight.SetActive(false);
        }
    }
    public void LoadSlide()
    {
        SceneManager.LoadScene("Slide");
    }
    public void LoadB1F()
    {
        SceneManager.LoadScene("B1F");
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

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


 
 
    public void LoadSlide()
    {
        SceneManager.LoadScene("Slide");
    }
    public void LoadB1F()
    {
        SceneManager.LoadScene("B1F 3");
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

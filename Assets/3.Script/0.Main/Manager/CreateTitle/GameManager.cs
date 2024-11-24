using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum ePlayerState
{
    Normal = 0,
    Minigame,
    Story,
    CutScene
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    


    [SerializeField] private string B1F;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
           
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnGameManagerLoaded;
        //FadeIn();  
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnGameManagerLoaded;
    }
    public void SetGameTimeScale(float value)
    {
        Time.timeScale = value;
    }
    private void OnGameManagerLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
    public void LoadSlide()
    {
        SceneManager.LoadScene("Slide");
    }
    public void LoadB1F()
    {
        GameSceneManager.Instance.FadeOut(B1F);
    }
    public void LoadScene(string SceneName)
    {
        GameSceneManager.Instance.FadeOut(SceneName);
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
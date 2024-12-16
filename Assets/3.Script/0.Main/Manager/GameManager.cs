using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public enum eGameType
{
    LoadGame = 0,
    NewGame
}
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

    
    public eGameType gameType;


    [SerializeField] private string B1F;

    private CinemachineBrain brain;
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
    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= OnGameManagerLoaded;
    }

    private void OnGameManagerLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera.main.TryGetComponent(out brain);
    }

    public void NextFrameChangeBlendTime(float blendTime)
    {
        StartCoroutine(ChangeBlendTime_co(blendTime));
    }
    public void CurrentFrameChangeBlendTime(float blendTime)
    {
        brain.m_DefaultBlend.m_Time = blendTime;
    }
    public void ResetBlendTime()
    {
        StartCoroutine(ChangeBlendTime_co(2f));
    }
    private IEnumerator ChangeBlendTime_co(float blendTime)
    {
        yield return null;
        brain.m_DefaultBlend.m_Time = blendTime;
        yield break;
    }
    public void LoadGame()
    {
        gameType = eGameType.LoadGame;
        LoadingManager.nextScene?.Invoke(B1F);
    }
    public void NewGame()
    {
        DataSaveManager.Instance.NewGame();
        gameType = eGameType.NewGame;
        LoadingManager.nextScene?.Invoke(B1F);
    }
    public void LoadLobby()
    {
        PlayerPrefs.Save();

        DataSaveManager.Instance.SaveGame();

        LoadingManager.nextScene?.Invoke("Lobby");
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
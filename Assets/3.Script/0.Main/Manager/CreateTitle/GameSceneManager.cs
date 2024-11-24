using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum eGameType
{
    LoadGame = 0,
    NewGame
}
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; } = null;

    private static string currentScene;


    private bool loadOnFadeOut;

    private string nextSceneName;

    private float fadeInTime;

    private bool justLoadedCheckRoom;

    private List<AsyncOperation> asyncUnloadOperationList = new List<AsyncOperation>();

    private bool button;
    private bool reloadPlayerScene;
    private int loadFadeFrameCount = 1;
    private static bool dontSave;
    private static bool reloadSaveOnLoad;
    private bool needToUnloadPlayer;
    private bool isCurrentlyLoading;

    [SerializeField] private Image fadePanel;
    [Range(0.1f, 5f)]
    [SerializeField] private float fadeTime;
    private float fade;
    private Coroutine fade_co;
    private bool isFadeOut;
    public eGameType gameType;

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
        fade = 1f;
        fadePanel.color = new Color(0f, 0f, 0f, fade);
        fade_co = null;
        currentScene = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name + " TotalScene Count: " + SceneManager.sceneCount);
        if (scene.name != "GameManager")
        {
            
       
            Debug.Log("Current Active Scene: " + scene.name);

            GameManager.Instance.SetGameTimeScale(1f);
            SceneManager.SetActiveScene(scene);
            if (fade.Equals(1f))
            {
                FadeIn();
                isFadeOut = false;
            }

        }
        else
        {
            Debug.Log("Current Active Scene (MISMATCH): " + scene.name + " Expected: " + currentScene);
        }


        
    }
    public void FadeIn()
    {
        if (fade_co == null)
        {
            if (TouchManager.Instance != null)
                TouchManager.Instance.EnableMoveHandler(false);

            fade_co = StartCoroutine(Fade_co(-1f));
        }
    }
    public void FadeOut(string SceneName, bool isSceneChange = false)
    {
        if (!isFadeOut)
        {
            if (fade_co != null)
            {
                StopCoroutine(fade_co);
            }
            isFadeOut = true;
            if (TouchManager.Instance != null)
                TouchManager.Instance.EnableMoveHandler(false);

            fade_co = StartCoroutine(Fade_co(1f, SceneName, isSceneChange));
        }
    }
    private IEnumerator Fade_co(float isFade, string SceneName = null, bool isSceneChange = false)
    {
        if (isSceneChange)
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
        while (true)
        {
            fade += isFade * Time.deltaTime / fadeTime;
            float fadeAlpha = Mathf.Lerp(0f, 1f, fade);
            fadePanel.color = new Color(0f, 0f, 0f, fadeAlpha);

            if (fadeAlpha.Equals(0f))
            {
                if (TouchManager.Instance != null)
                    TouchManager.Instance.EnableMoveHandler(true);
                fade = 0f;
                fade_co = null;
                yield break;
            }
            if (fadeAlpha.Equals(1f))
            {
                if (TouchManager.Instance != null)
                    TouchManager.Instance.EnableMoveHandler(true);
                fade = 1f;
                fade_co = null;
                isFadeOut = false;
                
                yield break;
            }
            yield return null;
        }
    }
    public static string GetCurrentSceneName()
    {
        return currentScene;
    }

    public void ReturnToTitle()
    {
        currentScene = "TitleScreen";
        FadeOut("Lobby", true);
    }
}

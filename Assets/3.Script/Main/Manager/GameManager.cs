using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeTime;
    private float fade;
    private Coroutine fade_co;


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

    private void OnGameManagerLoaded(Scene scene, LoadSceneMode mode)
    {
        fade = 1f;
        fadePanel.color = new Color(0f, 0f, 0f, fade);
        fade_co = null;
        FadeIn();
    }
    public void LoadSlide()
    {
        SceneManager.LoadScene("Slide");
    }
    public void LoadB1F()
    {
        SceneManager.LoadScene(B1F);
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void FadeIn()
    {
        if(fade_co == null)
        {
            if(TouchManager.Instance != null)
                TouchManager.Instance.EnableMoveHandler(false);

            fade_co = StartCoroutine(Fade_co(-1f));
        }
    }
    public void FadeOut()
    {
        if (fade_co == null)
        {
            if (TouchManager.Instance != null)
                TouchManager.Instance.EnableMoveHandler(false);

            fade_co = StartCoroutine(Fade_co(1f));
        }
    }
    private IEnumerator Fade_co(float isFade)
    {
        while(true)
        {
            fade += isFade * Time.deltaTime/fadeTime;
            float fadeAlpha = Mathf.Lerp(0f, 1f, fade);
            fadePanel.color = new Color(0f, 0f, 0f, fadeAlpha);

            if(fadeAlpha.Equals(0f)||fadeAlpha.Equals(1f))
            {
                if (TouchManager.Instance != null)
                    TouchManager.Instance.EnableMoveHandler(true);

                fade_co = null;
                yield break;
            }
            yield return null;
        }
    }
}

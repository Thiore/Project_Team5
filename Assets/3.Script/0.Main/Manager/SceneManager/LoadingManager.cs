using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; } = null;

    public static Action<string> nextScene;
    private string nextSceneName;

    private bool isDataLoaded;

    [SerializeField] private TMP_Text loadingProgress;

    [SerializeField] private Image fadePanel;
    [Range(0.1f, 5f)]
    [SerializeField] private float fadeTime;
    private float fade;
    private Coroutine fade_co;
    private bool isFadeOut;
    private bool isReadyScene;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            isDataLoaded = false;
            fade = 1f;
            fadePanel.color = new Color(0f, 0f, 0f, fade);
            fade_co = null;
            isReadyScene = false;
            isFadeOut = false;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Debug.Log("호출");
        SceneManager.sceneLoaded += LoadedScene;
        nextScene += LoadScene;
    }
  
    private void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= LoadedScene;
        nextScene -= LoadScene;
    }

    private void LoadedScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals("LoadingScene"))
            FadeIn();
    }

    private void LoadScene(string nextScene)
    {
        nextSceneName = nextScene;
        Debug.Log(nextSceneName);
        isReadyScene = true;
        FadeOut();
        
    }

    private IEnumerator LoadData_co()
    {
        yield return StartCoroutine(DataSaveManager.Instance.LoadItemData_co());

        yield return StartCoroutine(DataSaveManager.Instance.LoadGameSaveData_co());

        yield return StartCoroutine(DataSaveManager.Instance.LoadItemSaveData_co());

        isDataLoaded = true;
        yield return StartCoroutine(LoadNextScene("Lobby"));
    }

    private IEnumerator LoadNextScene(string SceneName)
    {
        // 비동기로 다음 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false;

        // 씬 로드 진행률 표시 (선택 사항)
        while (!asyncLoad.isDone)
        {
            float delayTime = 0;
            // progress가 0.9까지 도달할 때 로딩 진행률 갱신
            if (asyncLoad.progress >= 0.9f)
            {
               
                loadingProgress.text = "90%";
                // 추가 딜레이를 주고 allowSceneActivation 설정
                yield return new WaitForSeconds(0.1f); // 최소 대기 시간
                delayTime += 1f;
                loadingProgress.text = $"{90+ delayTime}%";
                if(delayTime>7f)
                {
                    FadeOut();
                }
                else if(delayTime>10f)
                {
                    if (!isFadeOut)
                    {
                        asyncLoad.allowSceneActivation = true;
                        FadeIn();
                    }
                }
                
                
               
                yield return null;
            }
            else
            {
                loadingProgress.text = $"{asyncLoad.progress * 100:F0}%";
            }
            
            yield return null;
        }

        Debug.Log("씬 전환 완료");
    }

    public void FadeIn()
    {
        if (fade_co == null)
        {
            if (TouchManager.Instance != null)
                TouchManager.Instance.EnableMoveHandler(true);

            fade_co = StartCoroutine(Fade_co(-1f));
        }
    }
    public void FadeOut()
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

            fade_co = StartCoroutine(Fade_co(1f));
        }
    }
    private IEnumerator Fade_co(float isFade)
    {
        if(isFade>0)
        {
            Debug.Log("out");
        }
            else
        {
            Debug.Log("in");
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
                if (SceneManager.GetActiveScene().name.Equals("LoadingScene"))
                {
                    loadingProgress.gameObject.SetActive(true);
                    loadingProgress.text = "0%";
                    if (!isDataLoaded)
                    {
                        StartCoroutine(LoadData_co());
                    }
                    else
                    {
                        StartCoroutine(LoadNextScene(nextSceneName));
                    }
                }

                fade_co = null;
                yield break;
            }
            if (fadeAlpha.Equals(1f))
            {
                if (TouchManager.Instance != null)
                    TouchManager.Instance.EnableMoveHandler(false);
                if (loadingProgress.gameObject.activeSelf)
                    loadingProgress.gameObject.SetActive(false);
                
                if(isReadyScene)
                {
                    isReadyScene = false;
                    SceneManager.LoadScene("LoadingScene");
                }
                fade_co = null;
                isFadeOut = false;
                yield break;
            }
            yield return null;
        }
    }
}

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

    [SerializeField] private TMP_Text loadingProgressText;
    [SerializeField] private Slider loadingProgressSlide;

    [SerializeField] private Image fadePanel;
    [Range(0.1f, 5f)]
    [SerializeField] private float fadeTime;
    private float fade;
    private Coroutine fade_co;
    private bool isLoadingScene;//로딩씬에서의 구분을 위해 사용
    private bool isFadeOut;
    private bool isFadeIn;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isDataLoaded = false;
            fade = 1f;
            fadePanel.color = new Color(0f, 0f, 0f, fade);
            fade_co = null;

            isLoadingScene = true;
            isFadeOut = false;
            isFadeIn = false;
            FadeIn(true);
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
        //SceneManager.sceneLoaded += LoadedScene;
        nextScene += LoadScene;
    }

    private void OnApplicationQuit()
    {
        //SceneManager.sceneLoaded -= LoadedScene;
        nextScene -= LoadScene;
    }

    //private void LoadedScene(Scene scene, LoadSceneMode mode)
    //{
    //    if(isLoading)
    //    {

    //        FadeIn();
    //    }

    //}

    private void LoadScene(string nextScene)
    {
        nextSceneName = nextScene;
        Debug.Log(nextSceneName);
        isLoadingScene = true;
        FadeOut(true);

    }

    private IEnumerator LoadData_co()
    {
        yield return StartCoroutine(DataSaveManager.Instance.LoadItemData_co());

        yield return StartCoroutine(DataSaveManager.Instance.LoadGameSaveData_co());

        yield return StartCoroutine(DataSaveManager.Instance.LoadItemSaveData_co());

        isDataLoaded = true;
        nextSceneName = "Lobby";
    }

    private IEnumerator LoadNextScene(string SceneName)
    {
        // 비동기로 다음 씬 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false;
        StopCoroutine(fade_co);
        fade_co = null;
        // 씬 로드 진행률 표시 (선택 사항)
        while (!asyncLoad.isDone)
        {

            // progress가 0.9까지 도달할 때 로딩 진행률 갱신
            if (asyncLoad.progress >= 0.9f)
            {
                int delayTime = 0;
                loadingProgressText.text = "70%";
                loadingProgressSlide.value = 70f;
                while (delayTime < 30)
                {
                    // 추가 딜레이를 주고 allowSceneActivation 설정
                    yield return new WaitForSeconds(0.1f); // 최소 대기 시간
                    delayTime += UnityEngine.Random.Range(0, 3);
                    loadingProgressText.text = $"{70 + delayTime}%";
                    loadingProgressSlide.value = 70f + delayTime;

                }
                isLoadingScene = false;

                

                FadeOut(true);
                while(isFadeOut)
                {
                    yield return null;
                }


                StopCoroutine(fade_co);
                fade_co = null;

                asyncLoad.allowSceneActivation = true;

            }
            else
            {
                loadingProgressText.text = $"{asyncLoad.progress * 100:F0}%";
                loadingProgressSlide.value = asyncLoad.progress * 100f;
            }

            yield return null;
        }
        FadeIn(true);
        while (isFadeIn)
        {
            yield return null;
        }        
        StopCoroutine(fade_co);
        fade_co = null;



        Debug.Log("씬 전환 완료");
    }

    public void FadeIn(bool isLoading = false)
    {
        if (fade_co != null)
        {
            StopCoroutine(fade_co);
            fade_co = null;
        }
        isFadeIn = true;
        if (isLoadingScene&&isLoading)
        {
            loadingProgressText.text = "0%";
            loadingProgressSlide.value = 0f;
            loadingProgressText.gameObject.SetActive(true);
            loadingProgressSlide.gameObject.SetActive(true);
        }

        fade_co = StartCoroutine(Fade_co(-1f, isLoading));
    }
    public void FadeOut(bool isLoading = false)
    {
        if(fade_co != null)
        {
            StopCoroutine(fade_co);
            fade_co = null;
        }
        
        isFadeOut = true;


        fade_co = StartCoroutine(Fade_co(1f, isLoading));
    }
    private IEnumerator Fade_co(float isFade, bool isLoading)
    {
        while (isFadeIn||isFadeOut)
        {
            fade += isFade * Time.deltaTime / fadeTime;
            fade = Mathf.Clamp(fade, 0f, 1f);
            fadePanel.color = new Color(0f, 0f, 0f, fade);

            if (fade <= 0f && isFadeIn)
            {
                

                if (isLoadingScene&& isLoading)
                {
                    if (!isDataLoaded)
                    {
                        yield return StartCoroutine(LoadData_co());
                    }
                    if (!nextSceneName.Equals("LoadingScene"))
                        StartCoroutine(LoadNextScene(nextSceneName));
                }
                isFadeIn = false;
                
            }
            if (fade >= 1f && isFadeOut)
            {
                
                if(isLoading)
                {
                    if (isLoadingScene)
                    {
                        SceneManager.LoadScene("LoadingScene");
                        FadeIn(true);
                    }
                    else
                    {
                        if (loadingProgressText.gameObject.activeInHierarchy)
                            loadingProgressText.gameObject.SetActive(false);
                        if (loadingProgressSlide.gameObject.activeInHierarchy)
                            loadingProgressSlide.gameObject.SetActive(false);
                    }
                }
                isFadeOut = false;
            }
            yield return null;
        }
    }

}

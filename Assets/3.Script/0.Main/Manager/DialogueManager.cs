using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; } = null;

    public string selectedLocale { get; private set; } //현재 선택된 언어
    private readonly string localeKey = "locale";

    public readonly string[] localeArray = { "en", "ko" };


    //스토리 관련 UI Text
    public Button dialogueButton; //대사 버튼 (터치 시, 사라지게 하기 위함)
    public TMP_Text dialogueText; //대사 표시할 TextMeshPro
    public TMP_Text speaker; // 현재 대사자
    private int currentIndex; // 현재 대화 인덱스
    private int endDialogueIndex; // 마지막 대화 인덱스
    private string dialogueTableName; // 대화 테이블 이름
    private string tempString;//넘겨줄 text

    //인벤토리 관련 Text
    private TMP_Text itemName; //인벤토리 아이템 이름 띄울 TextMeshPro
    private TMP_Text explanation; //인벤토리 아이템 설명 띄울 TextMeshPro

    

    private LocalizedString localizedString = new LocalizedString();
    private LocalizedString itemNameLocalizedString = new LocalizedString();
    private LocalizedString itemExplanationLocalizedString = new LocalizedString();
    private LocalizedString speakerLocalizedString = new LocalizedString(); // 스피커 로컬라이즈 문자열
    private LocalizedString tabletSpeakerLocalizedString = new LocalizedString();
    private LocalizedString tabletDialogueLocalizedString = new LocalizedString();

    public bool isDialogue { get; private set; } //현재 Dialogue가 활성화 중인지 여부

   
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WaitForLocalizationInitialization();
            if (dialogueButton.gameObject.activeInHierarchy)
                dialogueButton.gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
       
    }

    private void OnEnable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        //tabletSpeakerLocalizedString.StringChanged += UpdateDialogueText;
        //tabletDialogueLocalizedString.StringChanged += UpdateDialogueText;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //tabletSpeakerLocalizedString.StringChanged -= UpdateDialogueText;
        //tabletDialogueLocalizedString.StringChanged -= UpdateDialogueText;
    }

    //새 씬이 로드될 때 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitLocale();
        isDialogue = false;
        if (PlayerManager.Instance != null)
        {
            itemName = PlayerManager.Instance.getItemName;
            explanation = PlayerManager.Instance.getExplanation;
        }
    }

    //Story 테이블 이름과 키값을 통해 대사 출력
    public void SetDialogue(string tableName, int key)
    {
        isDialogue = true;
        // 버튼 및 움직임 비활성화
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(false); // 플레이어 버튼 상태 설정
        }
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableTouchHandle(false); // 움직임 비활성화
        }
        localizedString.TableReference = tableName; //테이블 이름 가져오기
        localizedString.TableEntryReference = key.ToString();
        localizedString.StringChanged += UpdateDialogueText;
        localizedString.RefreshString(); //번역된 문자열 업데이트

        StartCoroutine(StoryBottonState_co());
    }

    //대사 Text 업데이트
    private void UpdateDialogueText(string text)
    {
        dialogueText.text = text;

    }
    //대화창 활성/비활성화
    private IEnumerator StoryBottonState_co()
    {

        //2초 동안 버튼 비활성화
        dialogueButton.interactable = false;
        dialogueButton.gameObject.SetActive(true); //버튼 활성화
        yield return new WaitForSeconds(2f);

        //2초 후 버튼 활성화 및 터치 이벤트
        dialogueButton.interactable = true;
        dialogueButton.onClick.AddListener(OnButtonClicked);



        //7초 후 자동 비활성화(터치로 비활성화되지 않았을 경우)
        //yield return new WaitForSeconds(4f); //3초 대기 후 4초 더 대기
        //dialogueButton.gameObject.SetActive(false);
    }


    #region 플레이어 AI 대화
    //플레이어와 AI간 대화
    public void TalkStoryStart(int startIndex, int endIndex, string tableName, bool isSpeaker = true)
    {
        isDialogue = true;
        // 버튼 및 움직임 비활성화
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(false); // 플레이어 버튼 상태 설정
        }
        if(TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableTouchHandle(false); // 움직임 비활성화
        }
        
        //초기화
        currentIndex = startIndex;
        endDialogueIndex = endIndex;
        dialogueTableName = tableName;
        
        if(isSpeaker)
            speaker.gameObject.SetActive(true);
        //첫 번째 대사 출력
        ShowNextDialogue(isSpeaker);
    }
    private void ShowNextDialogue(bool isSpeaker = true)
    {
        if (currentIndex > endDialogueIndex) // 마지막 대사 이후라면 종료
        {
            EndDialogue();
            return;
        }

        // 대사 테이블과 키값을 설정
        localizedString.TableReference = dialogueTableName;
        localizedString.TableEntryReference = currentIndex.ToString(); // 현재 인덱스의 대사 가져오기
        localizedString.StringChanged += UpdateDialogueText;
        localizedString.RefreshString(); // 번역된 문자열 업데이트

        if(isSpeaker)
        {
            //스피커 업데이트
            UpdateSpeaker();
        }
        
        // 버튼 상태를 처리
        StartCoroutine(HandleDialogueButton_co());
    }
    private IEnumerator HandleDialogueButton_co()
    {
        dialogueButton.interactable = false;
        dialogueButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // 2초 후 버튼 활성화

        dialogueButton.interactable = true;
        dialogueButton.onClick.RemoveAllListeners();
        dialogueButton.onClick.AddListener(OnDialogueButtonClicked);
    }

    //스피커 업데이트
    private void UpdateSpeaker()
    {
        //홀수라면 key 19, 짝수라면 key 20
        int speakerKey = (currentIndex % 2 == 1) ? 19 : 20;
        // LocalizedString 설정
        speakerLocalizedString.TableReference = "UI"; // UI 테이블 사용
        speakerLocalizedString.TableEntryReference = speakerKey.ToString();
        speakerLocalizedString.StringChanged += UpdateSpeakerText;
        speakerLocalizedString.RefreshString();
    }
    //스피커 텍스트 업데이트
    private void UpdateSpeakerText(string text)
    {
        //스피커 UI 텍스트 변경
        speaker.text = text;
    }
    private void OnDialogueButtonClicked()
    {
        // 현재 대사의 리스너를 제거
        localizedString.StringChanged -= UpdateDialogueText;

        // 다음 대사로 이동
        currentIndex++;
        ShowNextDialogue();
    }
    private void EndDialogue()
    {
        
        if(speaker.gameObject.activeInHierarchy)
            speaker.gameObject.SetActive(false);

        dialogueButton.gameObject.SetActive(false); // 버튼 비활성화
        dialogueButton.onClick.RemoveAllListeners();
        // 버튼 및 움직임 비활성화
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true); // 플레이어 버튼 상태 설정
        }
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableTouchHandle(true); // 움직임 비활성화
        }
        isDialogue = false;
    }
    #endregion //


    private void OnButtonClicked()
    {
        // 버튼 및 움직임 비활성화
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true); // 플레이어 버튼 상태 설정
        }
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableTouchHandle(true); // 움직임 비활성화
        }
        
        //버튼 터치 시 즉시 비활성화
        dialogueButton.gameObject.SetActive(false);

        //터치 이벤트 제거
        dialogueButton.onClick.RemoveListener(OnButtonClicked);
        //btnList.SetActive(true);//인벤토리버튼 활성화
        isDialogue = false;
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //인벤토리 Item Id값과 Localization key 값 일치하게 만들어 출력
    public void SetItemNameText(string tableName, int key)
    {
        itemNameLocalizedString.TableReference = tableName; //테이블 이름 가져오기
        itemNameLocalizedString.TableEntryReference = key.ToString();
        itemNameLocalizedString.StringChanged += UpdateItemnameText;
        itemNameLocalizedString.RefreshString(); //번역된 문자열 업데이트
    }
    public void SetItemExplanationText(string tableName, int key)
    {
        itemExplanationLocalizedString.TableReference = tableName; //테이블 이름 가져오기
        itemExplanationLocalizedString.TableEntryReference = key.ToString();
        itemExplanationLocalizedString.StringChanged += UpdateItemExplanationText;
        itemExplanationLocalizedString.RefreshString(); //번역된 문자열 업데이트
    }
    private void UpdateItemnameText(string text)
    {
        itemName.text = text;
    }
    private void UpdateItemExplanationText(string text)
    {
        explanation.text = text;
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateDialogueText; //리스너 해제
    }


    
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //언어 변경
    public void ChangeLocale(int index)
    {
        if (index >= localeArray.Length)
        {
            return;
        }
        // 언어 바꿔주기 SelectedLocale에 있는 언어로
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeArray[index]);
        selectedLocale = localeArray[index];
        PlayerPrefs.SetString(localeKey, localeArray[index]);
        PlayerPrefs.Save();

    }

    //색상 업데이트
    //private void UpdateButtonTextColor(int index)
    //{
    //    if (index == 1)
    //    {
    //        koreanButtonText.color = activeColor;
    //        englishButtonText.color = inactiveColor;
    //    }
    //    else if (index == 0)
    //    {
    //        koreanButtonText.color = inactiveColor;
    //        englishButtonText.color = activeColor;
    //    }
    //}

    private async void WaitForLocalizationInitialization()
    {
        // InitializationOperation 가져오기
        var operation = LocalizationSettings.InitializationOperation;

        // 비동기 작업 완료 대기
        await operation.Task;

        if (operation.IsDone && !operation.Status.Equals(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed))
        {
            //Debug.Log("Localization 초기화 완료!");
            InitLocale();

            
        }
        else
        {
            //Debug.LogError("Localization 초기화 실패!");
        }
    }

    private void InitLocale()
    {
        if (PlayerPrefs.HasKey(localeKey))
        {
            selectedLocale = PlayerPrefs.GetString(localeKey);

        }
        else
        {
            selectedLocale = GetLocaleCodeFromSystemLanguage();
            PlayerPrefs.SetString(localeKey, selectedLocale);
            PlayerPrefs.Save();
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(selectedLocale);
    }

    private string GetLocaleCodeFromSystemLanguage()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean:
                return "ko"; // 한국어
            case SystemLanguage.English:
                return "en"; // 영어
            default:
                return "ko"; // 기본값 (영어)
        }
    }


    //text 업데이트
    public string UpdateText(string table, int index, bool isSpeaker)
    {
        if(isSpeaker)
        {
            tabletSpeakerLocalizedString.TableReference = table; // UI 테이블 사용
            tabletSpeakerLocalizedString.TableEntryReference = index.ToString();
            tabletSpeakerLocalizedString.RefreshString();

            return tabletSpeakerLocalizedString.GetLocalizedString();
        }
        else
        {
            tabletDialogueLocalizedString.TableReference = table; // UI 테이블 사용
            tabletDialogueLocalizedString.TableEntryReference = index.ToString();
            tabletDialogueLocalizedString.RefreshString();

            return tabletDialogueLocalizedString.GetLocalizedString(); ;
        }
    }
    //// text 업데이트
    //private void UpdateText(string text)
    //{
    //    if(text != null)
    //    {
    //        //텍스트 변경
    //        tempString = text;
    //    }
    //    else
    //    {
    //        Debug.Log("temp는 널");
    //    }    
        
    //}
}

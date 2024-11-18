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
    private DialogueManager instance = null;
    public static DialogueManager Instance { get; private set; }

    //스토리 관련 UI Text
    public Button dialogueButton; //대사 버튼 (터치 시, 사라지게 하기 위함)
    public TMP_Text dialogueText; //대사 표시할 TextMeshPro
    private GameObject playInterface;

    //인벤토리 관련 Text
    public TMP_Text itemName; //인벤토리 아이템 이름 띄울 TextMeshPro
    public TMP_Text explanation; //인벤토리 아이템 설명 띄울 TextMeshPro

    //옵션 관련 Text
    public TMP_Text koreanButtonText; //한국어 버튼
    public TMP_Text englishButtonText; //영어 버튼
    private Color activeColor = Color.green; //활성화 중인 언어 텍스트의 색상
    private Color inactiveColor = Color.white; //비활성화 중인 언어 텍스트의 색상

    private LocalizedString localizedString = new LocalizedString();
    private LocalizedString itemNameLocalizedString = new LocalizedString();
    private LocalizedString itemExplanationLocalizedString = new LocalizedString();

    private bool isChanging; //언어 변경
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
            
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
        if(PlayerManager.Instance != null)
            playInterface = PlayerManager.Instance.getInterface;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //새 씬이 로드될 때 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateButtonColorByLocale();
    }

    //Story 테이블 이름과 키값을 통해 대사 출력
    public void SetDialogue(string tableName, int key)
    {
        localizedString.TableReference = tableName; //테이블 이름 가져오기
        localizedString.TableEntryReference = key.ToString();
        localizedString.StringChanged += UpdateDialogueText;
        localizedString.RefreshString(); //번역된 문자열 업데이트

        StartCoroutine(StoryBottonState_co());
    }
    private void UpdateDialogueText(string text)
    {
        dialogueText.text = text;

    }



    private IEnumerator StoryBottonState_co()
    {
        if (playInterface != null)
        {
            //2초 동안 버튼 비활성화
            dialogueButton.interactable = false;
            dialogueButton.gameObject.SetActive(true); //버튼 활성화
            //playInterface.gameObject.SetActive(false); //인터페이스 비활성화
            yield return new WaitForSeconds(2f);

            //2초 후 버튼 활성화 및 터치 이벤트
            dialogueButton.interactable = true;
            dialogueButton.onClick.AddListener(OnButtonClicked);

            //if (!dialogueButton.gameObject.activeSelf)
            //{
            //    playInterface.gameObject.SetActive(true); //인터페이스 활성화
            //}
        }


        //7초 후 자동 비활성화(터치로 비활성화되지 않았을 경우)
        //yield return new WaitForSeconds(4f); //3초 대기 후 4초 더 대기
        //dialogueButton.gameObject.SetActive(false);
    }

    private void OnButtonClicked()
    {
        //버튼 터치 시 즉시 비활성화
        dialogueButton.gameObject.SetActive(false);

        //터치 이벤트 제거
        dialogueButton.onClick.RemoveListener(OnButtonClicked);
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
        if (isChanging)
            return;

        StartCoroutine(ChangeRoutine_co(index));

        
    }

    private IEnumerator ChangeRoutine_co(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation; //초기화

        // 언어 바꿔주기 SelectedLocale에 있는 언어로
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;

        //현재 언어에 따라 버튼 색상 업데이트
        UpdateButtonTextColor(index);

        
    }

    //색상 업데이트
    private void UpdateButtonTextColor(int index)
    {
        if (index == 1)
        {
            koreanButtonText.color = activeColor;
            englishButtonText.color = inactiveColor;
        }
        else if (index == 0)
        {
            koreanButtonText.color = inactiveColor;
            englishButtonText.color = activeColor;
        }
    }

    
   
    private void UpdateButtonColorByLocale()
    {
        // 현재 Locale의 인덱스를 가져와 버튼 색상 설정
        int currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        UpdateButtonTextColor(currentLocaleIndex);
    }

}

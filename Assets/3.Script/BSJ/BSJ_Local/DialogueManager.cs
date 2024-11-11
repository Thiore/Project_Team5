using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private DialogueManager instance = null;
    public static DialogueManager Instance { get; private set; }

    //스토리 관련 UI Text
    public Button dialogueButton; //대사 버튼 (터치 시, 사라지게 하기 위함)
    public TMP_Text dialogueText; //대사 표시할 TextMeshPro

    //인벤토리 관련 Text
    public TMP_Text itemName; //인벤토리 아이템 이름 띄울 TextMeshPro
    public TMP_Text explanation; //인벤토리 아이템 설명 띄울 TextMeshPro


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

        //텍스트 내용에 따라 크기 조정
        //StartCoroutine(DialogueSize_co());
    }



    private IEnumerator StoryBottonState_co()
    {
        //3초 동안 버튼 비활성화
        dialogueButton.interactable = false;
        dialogueButton.gameObject.SetActive(true); //버튼 활성화
        yield return new WaitForSeconds(3f);

        //3초 후 버튼 활성화 및 터치 이벤트
        dialogueButton.interactable = true;
        dialogueButton.onClick.AddListener(OnButtonClicked);

        //7초 후 자동 비활성화(터치로 비활성화되지 않았을 경우)
        yield return new WaitForSeconds(4f); //3초 대기 후 4초 더 대기
        dialogueButton.gameObject.SetActive(false);
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


    ////임시로 만들어 뒀습니다, 나중에 그냥 크기 고정 시키는 방향??이면 삭제
    //private IEnumerator DialogueSize_co()
    //{
    //    yield return null; //프레임 대기

    //    RectTransform rectTransform = dialogueText.GetComponent<RectTransform>();
    //    rectTransform.sizeDelta = new Vector2(dialogueText.preferredWidth, dialogueText.preferredHeight);
    //}

    //private void TextFinish()
    //{
    //    dialogueText.gameObject.SetActive(false);
    //}

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

        //dia.SetDialogue("B1", 22);
    }

    
}

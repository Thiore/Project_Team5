using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Button dialogueButton; //대사 버튼 (터치 시, 사라지게 하기 위함)
    public TMP_Text dialogueText; //대사 표시할 TextMeshPro
    private LocalizedString localizedString = new LocalizedString();

    // 테이블 이름과 키값을 통해 대사 출력
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

    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateDialogueText; //리스너 해제
    }


    //임시로 만들어 뒀습니다, 나중에 그냥 크기 고정 시키는 방향??이면 삭제
    private IEnumerator DialogueSize_co()
    {
        yield return null; //프레임 대기

        RectTransform rectTransform = dialogueText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(dialogueText.preferredWidth, dialogueText.preferredHeight);
    }

    //private void TextFinish()
    //{
    //    dialogueText.gameObject.SetActive(false);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText; //대사 표시할 TextMeshPro
    private LocalizedString localizedString = new LocalizedString();

    // 테이블 이름과 키값을 통해 대사 출력
    public void SetDialogue(string tableName, int key)
    {
        localizedString.TableReference = tableName; //테이블 이름 가져오기
        localizedString.TableEntryReference = key.ToString();
        localizedString.StringChanged += UpdateDialogueText;
        localizedString.RefreshString(); //번역된 문자열 업데이트
    }

    private void UpdateDialogueText(string text)
    {
        dialogueText.text = text;
    }

    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateDialogueText; //리스너 해제
    }
}

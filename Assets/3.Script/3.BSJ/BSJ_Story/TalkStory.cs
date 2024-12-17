using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkStory : MonoBehaviour
{
    [SerializeField] private int startIndex; //대사 시작 키값
    [SerializeField] private int endIndexl; //대사 끝 키값
    [SerializeField] private int playerIndex; //플레이어
    [SerializeField] private int aiIndex; //AI


    public void TalkStroyStart()
    {
        DialogueManager.Instance.TalkStoryStart(startIndex, endIndexl, "Table_ItemName", 0);

        //현재 인덱스 출력
        int currentDialougeIndex = DialogueManager.Instance.GetCurrentIndex();
        Debug.Log("현재 인덱스 : " + currentDialougeIndex);
            
    }

}

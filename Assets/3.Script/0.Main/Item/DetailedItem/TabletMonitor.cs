using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TabletMonitor : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject dialogueSpeakerPrefab;
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private Toggle[] mainManu;
    [Range(0.01f, 1f)]
    [SerializeField] private float waitTime;
    private int curIndex;
    private int endIndex;

    private string tempText;

    private const string tableUI = "UI";
    private const string tableDialogue = "Table_StoryB1";
    
    private bool isCut;
    public bool isDialogue { get; private set; }

    WaitForSeconds wait;


    private void Awake()
    {
        isDialogue = false;
        wait = new WaitForSeconds(waitTime);
    }
    
    public void SetDialogueIndex(int start, int end, bool cut = false)
    {
        isDialogue = true;
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(false); // 플레이어 버튼 상태 초기화
            TouchManager.Instance.EnableTouchHandle(false); // 움직임 다시 활성화
        }
        
        if (!mainManu[2].isOn)
        {
            mainManu[2].isOn = true;
        }
        foreach(Toggle manu in mainManu)
        {
            manu.interactable = false;
        }
        StartCoroutine(TabletText(start, end, cut));
    }

    private IEnumerator TabletText(int start, int end, bool cut)
    {
        curIndex = start;
        endIndex = end+1;

        while (curIndex < endIndex)
        {
            tempText = string.Empty;
            isCut = cut;
            yield return StartCoroutine(RevealText());

            ++curIndex;
            yield return null;
        }
        foreach (Toggle manu in mainManu)
        {
            manu.interactable = true;
        }
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true); // 플레이어 버튼 상태 초기화
            TouchManager.Instance.EnableTouchHandle(true); // 움직임 다시 활성화
        }
        isDialogue = false;
    }

    /// <summary>
    /// 한글자씩 출력되는 메서드
    /// </summary>
    /// <param name="tmp">사용할 TextMeshPro를 넣어주세요</param>
    /// <param name="text">text를 입력해주세요</param>
    /// <param name="waitSecond">시간초를 입력해주세요
    /// <para>기본값 : 0.1f, 0 : null</para></param>
    /// <returns></returns>
    private IEnumerator RevealText()
    {
        //홀수라면 key 19, 짝수라면 key 20
        int speakerKey = (curIndex % 2 == 1) ? 19 : 20;

        GameObject speaker = Instantiate(dialogueSpeakerPrefab, content);
        speaker.TryGetComponent(out TMP_Text speakerText);
        speakerText.text = string.Empty;
        tempText = DialogueManager.Instance.UpdateText(tableUI, speakerKey, true);
        if (speakerKey.Equals(19))
            speakerText.color = Color.green;
        else
            speakerText.color = Color.blue;
        speakerText.text = tempText;
        scroll.verticalNormalizedPosition = 0f;
        


        GameObject dialogue = Instantiate(dialoguePrefab, content);
        
        dialogue.TryGetComponent(out TMP_Text contentText);
        contentText.text = string.Empty;
        tempText = DialogueManager.Instance.UpdateText(tableDialogue, curIndex, false);
        Debug.Log(tempText);
        for (int i = 0; i < tempText.Length; ++i)
        {
            if(isCut)
            {
                contentText.text = tempText;
                scroll.verticalNormalizedPosition = 0f;
                yield break;
            }
            contentText.text += tempText[i];
            scroll.verticalNormalizedPosition = 0f;

            yield return wait;
        }
    }
    public void OnCut()
    {
        isCut = true;
    }
}

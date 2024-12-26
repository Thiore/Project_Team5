using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class TabletDialogue : MonoBehaviour
{
    public static TabletDialogue Instance { get; private set; }
    [SerializeField] private Transform content;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private TMP_Text dialogue;
    [Range(0.1f,1f)]
    [SerializeField] private float waitTime;

    private LocalizedString localizedString;

    private int curIndex;
    private int endIndex;

    
    private string tempDialogueText;

    private const string table = "Table_StoryB1";
    
    private bool isCut;

    WaitForSeconds wait;

    private void Awake()
    {
        Instance = this;
        wait = new WaitForSeconds(waitTime);
        localizedString = new LocalizedString();
    }
    
    public void SetDialogueIndex(int start, int end, bool cut = false)
    {
        StartCoroutine(TabletText(start, end, cut));
    }

    private IEnumerator TabletText(int start, int end, bool cut)
    {
        curIndex = start;
        endIndex = end+1;

        

        while (curIndex < endIndex)
        {
            
            isCut = cut;
            yield return StartCoroutine(RevealText());
            ++curIndex;
        }
    }

    /// <summary>
    /// 한글자씩 출력되는 메서드
    /// </summary>
    /// <param name="tmp">사용할 TextMeshPro를 넣어주세요</param>
    /// <param name="text">text를 입력해주세요</param>
    /// <param name="waitSecond">시간초를 입력해주세요
    /// <para>기본값 : 0.1f, 0 : null</para></param>
    /// <returns></returns>
    public IEnumerator RevealText()
    {
        //홀수라면 key 19, 짝수라면 key 20
        int speakerKey = (curIndex % 2 == 1) ? 19 : 20;
        tempDialogueText = DialogueManager.Instance.UpdateText("UI", speakerKey);
        TMP_Text tempSpeakerText = Instantiate(speakerName, content);
        tempSpeakerText.text = tempDialogueText;


        tempDialogueText = DialogueManager.Instance.UpdateText(table, curIndex);

        TMP_Text tempContentText = Instantiate(dialogue, content);

        tempContentText.text = string.Empty;

        for (int i = 0; i < tempDialogueText.Length; ++i)
        {
            if(isCut)
            {
                tempContentText.text = tempDialogueText;
                yield break;
            }
            tempContentText.text += tempDialogueText[i];
            yield return wait;
        }
    }

    public void CutReveal()
    {
        isCut = true;
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    // 자신의 인덱스를 가지게
    [SerializeField] private int storyIndex;

    //3D_UI Object
    [SerializeField] private GameObject clue;

    //3D_UI
    private GameObject ui_3D;

    private bool isStory = false; // 스토리 독백 상호작용 상태

    //private DialogueManager dialogueManager;

    private ReadInputData input;

    private void Start()
    {
        //DialougeManager 찾기
        //dialogueManager = FindObjectOfType<DialogueManager>();

        //3D_UI 찾기
        ui_3D = GameObject.FindGameObjectWithTag("3D_UI");

        //ReadInputData 찾기
        TryGetComponent(out input);
        if (DialogueManager.Instance == null)
        {
            Debug.Log("존재 하지 않아요");
        }
    }

    private void Update()
    {
        if (input.isTouch)
        {
            GetClue();
        }
    }


    //독백 Text 나오게
    private void StoryStart()
    {
        isStory = true; //상호작용 상태 

        //대사 출력
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetDialogue("Table_StoryB1", storyIndex);
        }


    }

    //단서 오브젝트 얻었을 때, 3D_UI 활성화
    private void GetClue()
    {
        ui_3D.gameObject.SetActive(true);
    }
}

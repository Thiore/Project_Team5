using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private int storyIndex; // 자신의 인덱스를 가지게
    private bool isStory = false; // 스토리 독백 상호작용 상태
    private DialogueManager dialogueManager;

    private void Start()
    {
        //DialougeManager 찾기
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.Log("존재 하지 않아요");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isStory)
        {
            StoryStart();
        }
    }

    //독백 Text 나오게
    private void StoryStart()
    {
        isStory = true; //상호작용 상태 

        //대사 출력
        if (dialogueManager != null)
        {
            dialogueManager.SetDialogue("Table_StoryB1", storyIndex);
        }
    }
}

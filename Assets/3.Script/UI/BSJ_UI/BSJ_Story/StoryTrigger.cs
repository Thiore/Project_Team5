using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    // 자신의 인덱스를 가지게
    [SerializeField] private int storyIndex;

    private ReadInputData input;

    private void Start()
    {
        //ReadInputData 가져오기
        TryGetComponent(out input);   
    }

    private void Update()
    {
        if (input.isTouch)
        {
            StoryStart();
        }
    }


    //독백 Text 나오게
    private void StoryStart()
    {
        //isStory = true; //상호작용 상태 

        //대사 출력
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetDialogue("Table_StoryB1", storyIndex);
        }


    }

    

   

   
}

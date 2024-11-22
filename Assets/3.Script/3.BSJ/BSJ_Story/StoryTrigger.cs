 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class StoryTrigger : MonoBehaviour, ITouchable
{
    // 자신의 인덱스를 가지게
    [SerializeField] private int storyIndex;



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

    public void OnTouchStarted(Vector2 position)
    {
        StoryStart();
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchEnd(Vector2 position)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : InteractionOBJ, ITouchable
{
    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;

    [Header("독백이 하나라면 동일한 Index를 입력해주세요")]
    [SerializeField] private int lockStartIndex;
    [SerializeField] private int lockEndIndex;

    [Header("독백이 하나라면 동일한 Index를 입력해주세요")]
    [SerializeField] private int startIndex;
    [SerializeField] private int endIndex;

    

    private bool isClear;

    protected override void Start()
    {
        base.Start();
        isTouching = false;

        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
    }


    public void OnTouchEnd(Vector2 position)
    {
        if (!isClear)
            isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (isClear)
                {
                    if(anim!= null)
                    {
                        isTouching = anim.GetBool(openAnim);
                        isTouching = !isTouching;
                        anim.SetBool(openAnim, isTouching);
                    }
                    
                }
                else
                {
                    //"잠겨있어"라는 독백 대사 출력
                    DialogueManager.Instance.TalkStoryStart(lockStartIndex, lockEndIndex, "Table_StoryB1", false);

                }


            }
        }
    }
    

    
    public void OnTouchStarted(Vector2 position)
    {
    }
    public void OnTouchHold(Vector2 position)
    {

    }
}

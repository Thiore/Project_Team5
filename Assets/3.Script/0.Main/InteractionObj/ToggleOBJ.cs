 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class ToggleOBJ : InteractionOBJ, ITouchable
{
    [Header("SaveManager 참고")]
    [SerializeField] protected int floorIndex;
    [SerializeField] protected int objectIndex;

    [Header("퍼즐 등 다른오브젝트와 상호작용이 필요하면 False")]
    [SerializeField] private bool isClear;

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        TryGetComponent(out anim);
        if (!isClear)
        {
            isClear = SaveManager.Instance.PuzzleState(floorIndex, objectIndex);

        }
    }

    public void OnTouchStarted(Vector2 position)
    {
    }
    public void OnTouchHold(Vector2 position)
    {

    }
    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if(!isClear)
                    isClear = SaveManager.Instance.PuzzleState(floorIndex, objectIndex);
               
                if (isClear)
                {
                    isTouching = !isTouching;
                    anim.SetBool(openAnim, isTouching);
                }
                else
                {
                    //"잠겨있어"라는 독백 대사 출력
                    DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
                    Debug.Log("여기 안 들어오나?");
                }
                
            }
        }
    }
    public void ClearOpen()
    {
        isTouching = true;
       
    }
    
    
}

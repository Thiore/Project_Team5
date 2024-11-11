using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour,ITouchable
{
    //상호작용 관련
    [SerializeField] private int floorIndex; //문이 있는 층 인덱스
    [SerializeField] private int objectIndex; // 상호작용 오브젝트 인덱스
    private SaveManager saveManager;
    private bool isTouchable;
    private Animator ani;
    

    


    //문이 열릴 수 있는 상태인지 확인 (퍼즐로 막혀 있는 문 or 격벽 정답 여부)
    private void CheckDoorState()
    {

        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        ani = GetComponent<Animator>();

        bool isinteracted = saveManager.PuzzleState(floorIndex, objectIndex);
        isTouchable = isinteracted;

    }

    //터치 가능 상태일 때
    private void EnableDoorInteraction()
    {
        ani.SetBool("Open", true);
    }

    //터치 불가능 상태일 때
    private void DisableDoorInteraction()
    {
        //"잠겨있어"라는 독백 대사 출력
        DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);            
        
    }
    private void OnEnable()
    {
        //OnEnable에서 자신이 열 수 있는 상태인지 아닌지 확인
        CheckDoorState();
    }

    public void OnTouchStarted(Vector2 position)
    {
        CheckDoorState();
        if (isTouchable)
        {
            Debug.Log("여기");
            EnableDoorInteraction();
        }
        else
        { 
            DisableDoorInteraction();
            Debug.Log("이러면 안된다...");
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchEnd(Vector2 position)
    {
        
    }

}

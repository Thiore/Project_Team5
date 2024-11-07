using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //상호작용 관련
    [SerializeField] private int floorIndex; //문이 있는 층 인덱스
    [SerializeField] private int objectIndex; // 상호작용 오브젝트 인덱스
    private SaveManager saveManager;
    private bool isTouchable;


    private Animator ani;
    private ReadInputData input;
    

    

    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();

        ani = GetComponent<Animator>();

        TryGetComponent(out input);

    }

    private void Update()
    {
        CheckDoorState();
    }

    //문이 열릴 수 있는 상태인지 확인 (퍼즐로 막혀 있는 문 or 격벽 정답 여부)
    private void CheckDoorState()
    {
        //해당 층과 오브젝트 인덱스에 해당하는 isInteracted 상태 확인
        StateData.FloorState floor = saveManager.gameState.floors.Find(f => f.floorIndex == floorIndex);

        if (floor != null)
        {
            //해당 오브젝트의 상태를 확인
            StateData.InteractableObjectState doorState = floor.interactableObjects.Find(obj => obj.objectIndex == objectIndex);

            if (doorState != null)
            {
                //isInteracted 상태 확인 후, true라면 문을 터치 가능 상태로
                isTouchable = doorState.isInteracted;

                if (input.isTouch)
                {
                    if (isTouchable)
                    {
                        EnableDoorInteraction();
                        Debug.Log("성공");
                    }
                    else if(!isTouchable)
                    {
                        DisableDoorInteraction();
                        Debug.Log("실패");
                    }
                }
            }
        }
    }

    //터치 가능 상태일 때
    private void EnableDoorInteraction()
    {
        if (isTouchable)
        {
            if (input.isTouch)
            {
                //ani.SetTrigger("Open");
                ani.SetBool("isOpen", true);
            }
        }
    }

    //터치 불가능 상태일 때
    private void DisableDoorInteraction()
    {
        //"잠겨있어"라는 독백 대사 출력
        if (!isTouchable)
        {
            if (input.isTouch)
            {
                DialogueManager.Instance.SetDialogue("Table_StoryB1", 0);
            }
        }
    }
}

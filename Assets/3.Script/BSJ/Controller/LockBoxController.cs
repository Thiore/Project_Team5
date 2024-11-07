using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBoxController : MonoBehaviour
{
    [SerializeField] private int floorIndex; //오브젝트의 현재 층
    [SerializeField] private int objectIndex; // 상호작용 오브젝트 인덱스
    private SaveManager saveManager;
    private Animator ani;
    private ReadInputData input;
    private bool isAnswer; //상호작용 오브젝트 정답여부


    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        ani = GetComponent<Animator>();
        TryGetComponent(out input);
    }

    private void Update()
    {
        CheckBoxState();
    }

    //상호작용 오브젝트 상태에 따른 상자 상태 여부
    private void CheckBoxState()
    {
        //해당 층과 오브젝트 인덱스에 해당하는 isInteracted 상태 확인
        StateData.FloorState floor = saveManager.gameState.floors.Find(f => f.floorIndex == floorIndex);

        if (floor != null)
        {
            //해당 오브젝트의 상태를 확인
            StateData.InteractableObjectState boxState = floor.interactableObjects.Find(obj => obj.objectIndex == objectIndex);

            if (boxState != null)
            {
                isAnswer = boxState.isInteracted;
                if (isAnswer)
                {
                    ani.SetTrigger("Open");
                }
            }
        }
    }
}

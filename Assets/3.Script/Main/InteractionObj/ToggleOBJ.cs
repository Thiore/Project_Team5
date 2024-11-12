using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : MonoBehaviour, ITouchable
{
    [Header("isClear가 True일때 사용할 카메라")]
    [SerializeField] private GameObject clearCamera;
    [Header("isClear가 false일때 사용할 카메라")]
    [SerializeField] private GameObject cinemachine;

    private Animator anim;
    private readonly int openAnim = Animator.StringToHash("Open");
    private Outline outline;

    private bool isTouching;

    [Header("퍼즐 등 다른오브젝트와 상호작용이 필요하면 False")]
    [SerializeField] private bool isClear;

    //SaveManger 참조
    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;
    private SaveManager saveManager;
    

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;

        if (!TryGetComponent(out anim))
        {
            transform.parent.TryGetComponent(out anim);
        }
        if (!isClear)
        {
            
        }

        isTouching = false;
    }

    public void OnTouchStarted(Vector2 position)
    {
        //SaveManager isinteracted(퍼즐 결과 연동)
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        bool isinteracted = saveManager.PuzzleState(floorIndex, objectIndex);
        isClear = isinteracted;

        isTouching = !isTouching;
        if (isClear)
        {
            clearCamera.SetActive(isTouching);
            anim.SetBool(openAnim, isTouching);
        }
        else
        {
            cinemachine.SetActive(isTouching);
        }



    }
    public void OnTouchHold(Vector2 position)
    {

    }
    public void OnTouchEnd(Vector2 position)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = false;
        }
    }
}

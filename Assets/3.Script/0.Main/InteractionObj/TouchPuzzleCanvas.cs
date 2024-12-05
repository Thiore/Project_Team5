using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPuzzleCanvas : MonoBehaviour,ITouchable
{
    [SerializeField] private GameObject missionStart;
    [SerializeField] private GameObject missionExit;
    [Header("게임이 끝난 후 상호작용이 가능한 오브젝트들")]
    [SerializeField] private GameObject interactionCam;

    [SerializeField] private GameObject btnExit;

    [SerializeField] private PlayOBJ playPuzzle;

    [Header("SaveManager 참고")]
    [SerializeField] private int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] private int objectIndex;

    [Header("상호작용 아이템이 있다면 추가해주세요")]
    [SerializeField] private List<int> interactionIndex;
    public List<int> getInteractionIndex { get => interactionIndex; }

    private Animator anim;

    [SerializeField] private Animator[] interactionAnim;
    private readonly int openAnim = Animator.StringToHash("Open");

    private Collider mask;
    [HideInInspector]
    public bool isClear;
    [HideInInspector]
    public bool isInteracted;

    private GameObject btnList;
    //private UseButton quickSlot;

    private Outline outline;

    private void OnEnable()
    {
        if (!DataSaveManager.Instance.GetGameState(floorIndex, objectIndex))
        {
            isClear = false;
        }
        else
        {
            isClear = true;
            if (interactionAnim.Length > 0)
            {
                for (int i = 0; i < interactionAnim.Length; i++)
                {
                    interactionAnim[i].SetBool(openAnim, true);
                    if (TryGetComponent(out ToggleOBJ toggleObj))
                    {
                        toggleObj.ClearOpen();
                    }
                }
            }
            return;
        }

        

       

        if (interactionIndex.Count.Equals(0))
        {
            isInteracted = true;
        }
        else
        {
            for(int i = interactionIndex.Count-1; i >= 0;--i)
            {
                if(!DataSaveManager.Instance.GetGameState(floorIndex, interactionIndex[i]))
                {
                    isInteracted = false;
                    break;
                }
                else
                {
                    interactionIndex.RemoveAt(i);
                }
                isInteracted = true;
            }
        }
    }

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
        TryGetComponent(out mask);
        TryGetComponent(out anim);
    }

    
    public void OffInteraction()
    {
        
        mask.enabled = true;
        if(btnExit != null)
            btnExit.SetActive(false);

        if(UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }

        if (!isClear)
        {
            missionStart.SetActive(false);
            missionExit.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, isClear);
            }
            TouchManager.Instance.EnableMoveHandler(true);
        }
        else
        {
            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, isClear);
            }
            Invoke("ClearEvent", 2f);
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

        if (isClear) return;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (!isInteracted && !UI_InvenManager.Instance.HaveItem(interactionIndex))
                {
                    DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
                }
                else if (!isInteracted)
                {
                    if(missionStart.activeInHierarchy)
                    {
                        DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
                    }
                    else
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);
                        UI_InvenManager.Instance.OpenQuickSlot();
                    }
                }
                else
                {
                    TouchManager.Instance.EnableMoveHandler(false);
                    missionStart.SetActive(true);
                    missionExit.SetActive(true);
                    btnExit.SetActive(true);

                    if(UI_InvenManager.Instance.isOpenQuick)
                    {
                        UI_InvenManager.Instance.CloseQuickSlot();
                    }

                    if (anim != null)
                    {
                        anim.SetBool(openAnim, true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClear) return;

        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }

    private void ClearEvent()
    {
        for(int i = 0; i < interactionAnim.Length;i++)
        {
            interactionAnim[i].SetBool(openAnim, true);
        }
        Invoke("ResetCamera", 1f);
    }
    private void ResetCamera()
    {
        missionExit.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        btnList.SetActive(true);
        //quickSlot.QucikSlotButton(true);
    }

    public void InteractionObject(int obj)
    {
        interactionIndex.Remove(obj);
        if(interactionIndex.Count.Equals(0))
        {
            isInteracted = true;
        }
    }
}

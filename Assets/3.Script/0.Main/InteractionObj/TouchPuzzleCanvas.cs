using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchPuzzleCanvas : MonoBehaviour,ITouchable
{
    [SerializeField] protected GameObject missionStart;
    [SerializeField] protected GameObject missionExit;
    [Header("Outline의 상호작용을 할 카메라")]
    [SerializeField] protected GameObject interactionCam;

    [SerializeField] protected GameObject btnExit;

    [Header("SaveManager 참고")]
    [SerializeField] protected int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] protected int objectIndex;
    public int getObjectIndex { get => objectIndex; }

    [Header("퀵슬롯상호작용 아이템이 있다면 추가해주세요")]
    [SerializeField] protected List<int> interactionIndex;
    public List<int> getInteractionIndex { get => interactionIndex; }

    [SerializeField] protected Animator anim;

    [SerializeField] protected Animator[] interactionAnim;
    protected readonly int openAnim = Animator.StringToHash("Open");

    [SerializeField] protected Collider mask;
    [HideInInspector]
    public bool isClear;
    [HideInInspector]
    public bool isInteracted;

    public bool isStarted { get; protected set; }

    [Header("사라져야할 아이템이 있다면 체크해주세요")]
    [SerializeField] private bool isRemoveClue;
    [SerializeField] private int removeClueId;


    protected Outline outline;

    protected virtual void OnEnable()
    {
        if (!DataSaveManager.Instance.GetGameState(floorIndex, objectIndex))
        {
            isClear = false;
            isStarted = false;
        }
        else
        {
            isClear = true;
            isStarted = false;
            isInteracted = true;
            if (interactionAnim.Length > 0)
            {
                for (int i = 0; i < interactionAnim.Length; i++)
                {
                    interactionAnim[i].SetBool(openAnim, true);
                    
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

    protected virtual void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
        if(mask == null)
            TryGetComponent(out mask);
        if(anim == null)
            TryGetComponent(out anim);
    }

    
    public virtual void OffInteraction()
    {
        
        
        if(btnExit != null)
            btnExit.SetActive(false);

        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
        if (isClear && isRemoveClue)
        {
            if (UI_InvenManager.Instance.HaveItem(removeClueId))
            {
                UI_InvenManager.Instance.SortInvenSlot(removeClueId);
                isRemoveClue = false;

            }

        }
       
    }

    public void OnTouchStarted(Vector2 position)
    { 
    }

    public void OnTouchHold(Vector2 position)
    { 
    }

    public abstract void OnTouchEnd(Vector2 position);

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }



    public virtual void InteractionObject(int id)
    {
        DataSaveManager.Instance.UpdateGameState(floorIndex, id, true);
        interactionIndex.Remove(id);
        if(interactionIndex.Count.Equals(0))
        {
            isInteracted = true;
            
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
        }
    }
    protected abstract void ClearEvent();

    protected abstract void ResetCamera();
}

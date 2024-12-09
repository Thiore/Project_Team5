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

    [SerializeField] protected PlayOBJ playPuzzle;

    [Header("SaveManager 참고")]
    [SerializeField] protected int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] protected int objectIndex;

    [Header("퀵슬롯상호작용 아이템이 있다면 추가해주세요")]
    [SerializeField] protected List<int> interactionIndex;
    public List<int> getInteractionIndex { get => interactionIndex; }

    protected Animator anim;

    [SerializeField] protected Animator[] interactionAnim;
    protected readonly int openAnim = Animator.StringToHash("Open");

    protected Collider mask;
    [HideInInspector]
    public bool isClear;
    [HideInInspector]
    public bool isInteracted;

    protected GameObject btnList;
    //private UseButton quickSlot;

    protected Outline outline;

    protected void OnEnable()
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

    protected void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
        TryGetComponent(out mask);
        TryGetComponent(out anim);
    }

    
    public virtual void OffInteraction()
    {
        
        
        if(btnExit != null)
            btnExit.SetActive(false);

        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

       
    }

    public void OnTouchStarted(Vector2 position)
    { 
    }

    public void OnTouchHold(Vector2 position)
    { 
    }

    public virtual void OnTouchEnd(Vector2 position)
    {

        if (isClear) return;
        
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }



    public void InteractionObject(int obj)
    {
        interactionIndex.Remove(obj);
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

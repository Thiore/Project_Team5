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

    [Header("상호작용 아이템이 없다면 -1")]
    [SerializeField] private int[] InteractionIndex;
    public int[] getInteractionIndex { get => InteractionIndex; }

    private Animator anim;

    [SerializeField] private Animator[] interactionAnim;
    private readonly int openAnim = Animator.StringToHash("Open");

    private Collider mask;
    [HideInInspector]
    public bool isClear;
    [HideInInspector]
    public bool isInteracted;
    [HideInInspector]
    public bool isInteractionCam;

    private GameObject btnList;
    private UseButton quickSlot;

    

    

    private Outline outline;

    private void OnEnable()
    {
        btnList = PlayerManager.Instance.getBtnList;
        quickSlot = PlayerManager.Instance.getQuickSlot;
        
            if(!SaveManager.Instance.PuzzleState(floorIndex, objectIndex))
            {
                isClear = false;
            }
            else
        {
            isClear = true;
        }
            
        
        if (InteractionIndex.Length.Equals(0))
        {
            isInteracted = true;
        }
        else
        {
            for(int i = 0; i < InteractionIndex.Length;i++)
            {
                if(!SaveManager.Instance.PuzzleState(floorIndex, InteractionIndex[i]))
                {
                    isInteracted = false;
                    break;
                }
                isInteracted = true;
            }
        }
            
       
     
        if (isClear&&interactionAnim.Length>0)
        {
            for (int i = 0; i < interactionAnim.Length; i++)
            {
                interactionAnim[i].SetBool(openAnim, true);
                if(TryGetComponent(out ToggleOBJ toggleObj))
                {
                    toggleObj.ClearOpen();
                }
            }
        }

        isInteractionCam = false;
    }

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
        TryGetComponent(out mask);
        TryGetComponent(out anim);
    }

    
    public void OffKeypad()
    {
        
        mask.enabled = true;
        btnExit.SetActive(false);

        if(!isClear)
        {
            isClear = SaveManager.Instance.PuzzleState(floorIndex, objectIndex);
        }

        if (isClear)
        {
            missionExit.SetActive(true);
            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, true);
            }
            Invoke("ClearEvent", 2f);
        }
        else
        {
            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
            }
            TouchManager.Instance.EnableMoveHandler(true);
            
            btnList.SetActive(true);
            
            quickSlot.QucikSlotButton(true);
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

        if (isClear)
        {
            if (interactionCam != null)
            {
                isInteractionCam = !isInteractionCam;
                interactionCam.SetActive(isInteractionCam);
                quickSlot.QucikSlotButton(!isInteractionCam);
            }

            return;
        }

        if(!isInteracted)
        {
            DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
               
                    TouchManager.Instance.EnableMoveHandler(false);
                    missionStart.SetActive(true);

                    mask.enabled = false;

                    btnExit.SetActive(true);

                if(playPuzzle!=null&&playPuzzle.getInteractionCount> 0)
                {
                    btnList.SetActive(false);
                }
                else
                {
                    btnList.SetActive(false);
                }
                    quickSlot.QucikSlotButton(false);

                if(anim != null)
                {
                    anim.SetBool(openAnim, true);
                }
                
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClear)
        {
            if (interactionCam == null)
            {
                return;
            }

            
        }
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isClear)
        {
            if(interactionCam == null||outline != null)
            {
                outline.enabled = false;
                return;
            }
        }

       
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
        quickSlot.QucikSlotButton(true);
    }
    public void SetQuickSlot()
    {
        if (playPuzzle.getInteractionCount>0)
        {
            quickSlot.QucikSlotButton(true);
        }
    }
    

}

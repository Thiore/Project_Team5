using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPuzzleCanvas : MonoBehaviour,ITouchable
{
    [SerializeField] private GameObject missionStart;
    [SerializeField] private GameObject missionExit;

    [SerializeField] private GameObject btnExit;

    [SerializeField] private PlayOBJ playPuzzle;

    [Header("SaveManager 참고")]
    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;

    [Header("상호작용 아이템이 없다면 0")]
    [SerializeField] private int InteractionIndex;

    private Animator anim;

    [SerializeField] private Animator[] interactionAnim;
    private readonly int openAnim = Animator.StringToHash("Open");

    private Collider mask;
    [HideInInspector]
    public bool isClear;
    [HideInInspector]
    public bool isInteracted;

    private List<GameObject> defaultObj; //게임이 시작되면 기본적으로 꺼져야하는것들
    public GameObject quickSlot { get; private set; }
    

    

    private Outline outline;

    private void OnEnable()
    {
        Transform playerInterface = PlayerManager.Instance.getInterface.transform;
        defaultObj = new List<GameObject>();
        for(int i = 0; i < playerInterface.childCount;i++)
        {
            if (playerInterface.GetChild(i).CompareTag("KeyClue"))
            {
                continue;
            }
            else if (playerInterface.GetChild(i).CompareTag("QuickSlot"))
            {
                quickSlot = playerInterface.GetChild(i).gameObject;
            }
            else
            {
                defaultObj.Add(playerInterface.GetChild(i).gameObject);
            }
        }

        isClear = SaveManager.Instance.PuzzleState(floorIndex, objectIndex);

        isInteracted = SaveManager.Instance.PuzzleState(floorIndex, InteractionIndex);
     
        if (isClear&&interactionAnim.Length>0)
        {
            for (int i = 0; i < interactionAnim.Length; i++)
            {
                interactionAnim[i].SetBool(openAnim, true);
            }
        }
    }

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
        TryGetComponent(out mask);
        isClear = false;
        TryGetComponent(out anim);
    }

    
    public void OffKeypad()
    {
        
        mask.enabled = true;
        btnExit.SetActive(false);

        if (isClear)
        {
            missionExit.SetActive(true);
            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
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
            for (int i = 0; i < defaultObj.Count; i++)
            {
                defaultObj[i].SetActive(true);
            }
            quickSlot.SetActive(true);
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
        if(!isInteracted)
        {
            DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
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

                if(playPuzzle .getInteractionCount> 0)
                {
                    for(int i = 0; i < defaultObj.Count;i++)
                    {
                        defaultObj[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < defaultObj.Count; i++)
                    {
                        defaultObj[i].SetActive(false);
                    }
                    quickSlot.SetActive(false);
                }

                if(anim != null)
                {
                    anim.SetBool(openAnim, true);
                }
                
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClear) return;
        if (other.CompareTag("MainCamera"))
        {
            outline.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isClear) return;
        if (other.CompareTag("MainCamera"))
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
        Invoke("ResetCamera", 2f);
    }
    private void ResetCamera()
    {
        missionExit.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        for (int i = 0; i < defaultObj.Count; i++)
        {
            defaultObj[i].SetActive(true);
        }
        quickSlot.SetActive(true);
    }
    public void SetQuickSlot()
    {
        if (playPuzzle.getInteractionCount.Equals(0))
        {
            quickSlot.SetActive(false);
        }
    }
    

}

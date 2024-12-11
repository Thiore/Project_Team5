using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : InteractionOBJ, ITouchable
{
    [Header("DataSaveManager 참고")]
    [SerializeField] private int floorIndex;
    public int getFloorIndex { get => floorIndex; }
    [SerializeField] private int objectIndex;
    public int getObjectIndex { get => objectIndex; }

    [SerializeField] private int lockIndex;
    [SerializeField] private int dontInteractionIndex;


    [Header("퍼즐 또는 퀵슬롯의 다른오브젝트와 상호작용이 필요하면 False")]
    [SerializeField] private bool isClear;

    private Coroutine closeCam_co = null;

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        TryGetComponent(out anim);
        if (!isClear)
        {
            isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
            
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
        if(closeCam_co == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

                    if (isClear)
                    {
                        isTouching = !isTouching;
                        if (normalCamera != null)
                        {
                            normalCamera.SetActive(isTouching);
                            anim.SetBool(openAnim, isTouching);
                            if (PlayerManager.Instance != null)
                            {
                                PlayerManager.Instance.SetBtn(!isTouching);
                            }
                            if (TouchManager.Instance != null)
                            {
                                TouchManager.Instance.EnableMoveHandler(!isTouching);
                            }
                        }
                        else
                        {
                            anim.SetBool(openAnim, isTouching);
                        }
                    }
                    else
                    {
                        if (normalCamera != null)
                        {
                            if (UI_InvenManager.Instance.HaveItem(objectIndex))
                            {
                                if (!normalCamera.activeInHierarchy)
                                {
                                    normalCamera.SetActive(true);
                                    if (PlayerManager.Instance != null)
                                    {
                                        PlayerManager.Instance.SetBtn(false);
                                    }
                                    if (TouchManager.Instance != null)
                                    {
                                        TouchManager.Instance.EnableMoveHandler(false);
                                    }

                                    UI_InvenManager.Instance.OpenQuickSlot();
                                }
                                else
                                {
                                    //"잠겨있어"라는 독백 대사 출력
                                    DialogueManager.Instance.SetDialogue("Table_StoryB1", lockIndex);
                                    closeCam_co = StartCoroutine(CloseInteractionCam_co());
                                }
                            }
                            else
                            {
                                DialogueManager.Instance.SetDialogue("Table_StoryB1", dontInteractionIndex);
                            }
                               

                        }
                    }
                }
            }
        }
        
    }
    
    private IEnumerator CloseInteractionCam_co()
    {
        while(DialogueManager.Instance.isDialogue)
        {
            yield return null;
        }
        normalCamera.SetActive(false);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableMoveHandler(true);
        }

        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }
        closeCam_co = null;
    }

    public void InteractionObject()
    {
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex, true);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.EnableMoveHandler(true);
        }
        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }
        normalCamera.SetActive(false);
    }
    
}

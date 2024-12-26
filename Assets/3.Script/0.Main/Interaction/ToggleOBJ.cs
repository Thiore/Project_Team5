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
    [SerializeField] private int startIndex;
    [SerializeField] private int endIndex;


    private bool isClear;

    private Coroutine closeCam_co = null;
    [Header("트리거 아이템으로 설정하면 바로 상호작용 되는 오브젝트는 true")]
    [SerializeField] private bool isHaveTrriger;
    [SerializeField] private TriggerButton trigger;
    [SerializeField] private GameObject CutScene;

    [Header("트리거 아이템으로 설정하면 바로 상호작용 되는 오브젝트는 true")]
    [SerializeField] private bool isHaveInven;


    protected override void Start()
    {
        base.Start();
        isTouching = false;
        if(!TryGetComponent(out anim))
        {
            transform.parent.TryGetComponent(out anim);
        }
        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
        
            
            
        
    }

    public void OnTouchStarted(Vector2 position)
    {
    }
    public void OnTouchHold(Vector2 position)
    {

    }
    public void OnTouchEnd(Vector2 position)
    {
        if (closeCam_co == null)
        {
            if(!isClear)
                isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    if (!isHaveTrriger && !isHaveInven)
                    {
                        if (isClear)
                        {
                            isTouching = anim.GetBool(openAnim);
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

                            //"잠겨있어"라는 독백 대사 출력
                            DialogueManager.Instance.TalkStoryStart(startIndex,endIndex,"Table_StoryB1", false);
                            closeCam_co = StartCoroutine(CloseInteractionCam_co());
                        }
                    }
                    else if (isHaveInven)
                    {
                        if(isClear)
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
                            if (UI_InvenManager.Instance.HaveItem(objectIndex))
                            {
                                isTouching = !isTouching;
                                normalCamera.SetActive(isTouching);
                                if(isTouching)
                                {
                                    UI_InvenManager.Instance.OpenQuickSlot();
                                }
                                else
                                {
                                    UI_InvenManager.Instance.CloseQuickSlot();
                                }
                                
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
                                DialogueManager.Instance.SetDialogue("Table_StoryB1", lockIndex);
                                //lockIndex = 어떤아이템이 필요해
                                closeCam_co = StartCoroutine(CloseInteractionCam_co());
                            }
                        }
                        

                    }
                    else
                    {
                        if (trigger.item.id.Equals(objectIndex))
                        {
                            DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
                            CutScene.SetActive(true);
                        }
                        else
                        {
                            DialogueManager.Instance.SetDialogue("Table_StoryB1", lockIndex);
                            //lockIndex = 어떤아이템이 필요해
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
        if(normalCamera != null)
            normalCamera.SetActive(false);


        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }
        closeCam_co = null;
    }

    public void InteractionObject()
    {
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex, true);
        
        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }
        if (anim != null)
        {
            anim.SetBool(openAnim, true);
        }
    }
    
}

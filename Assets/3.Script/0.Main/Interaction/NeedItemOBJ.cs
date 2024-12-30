using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedItemOBJ : InteractionOBJ, ITouchable
{
    private enum eNeedItem
    {
        have = 0,
        trigger,
        quick
    }

    [SerializeField] private eNeedItem needItem;
    [Header("needItem이 trigger라면 trigger버튼을 참조해주세요")]
    [SerializeField] private TriggerButton trigger;

    [Header("컷신등 게임진행 중 활성화해줘야하는 오브젝트")]
    [SerializeField] private GameObject[] enableObj;

    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;
    public int getObjectIndex { get => objectIndex; }

    [Header("독백이 하나라면 동일한 Index를 입력해주세요")]
    [SerializeField] private int lockStartIndex;
    [SerializeField] private int lockEndIndex;
    [Header("독백이 하나라면 동일한 Index를 입력해주세요")]
    [SerializeField] private int startIndex;
    [SerializeField] private int endIndex;

    private bool isClear;

    private Coroutine normalCam_co = null;

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
    }



    public void OnTouchEnd(Vector2 position)
    {
        if (!isClear)
            isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

        if (normalCam_co == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    switch (needItem)
                    {
                        case eNeedItem.have:
                            if(UI_InvenManager.Instance.HaveItem(objectIndex))
                            {
                                DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
                                UI_InvenManager.Instance.SortInvenSlot(objectIndex);
                                isClear = true;
                            }
                            else
                            {
                                DialogueManager.Instance.TalkStoryStart(lockStartIndex, lockEndIndex, "Table_StoryB1", false);
                            }
                            if(isClear)
                            {
                                isTouching = !isTouching;
                                anim.SetBool(openAnim, isTouching);
                            }
                            
                            break;
                        case eNeedItem.trigger:
                            if (trigger.item.id.Equals(objectIndex))
                            {
                                DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
                                foreach (GameObject obj in enableObj)
                                {
                                    obj.SetActive(true);
                                }
                            }
                            else
                            {
                                DialogueManager.Instance.TalkStoryStart(lockStartIndex, lockEndIndex, "Table_StoryB1", false);
                            }
                            break;
                        case eNeedItem.quick:
                            if (!isClear)
                            {
                                if (UI_InvenManager.Instance.HaveItem(objectIndex))
                                {
                                    isTouching = !isTouching;
                                    normalCamera.SetActive(isTouching);
                                    if (isTouching)
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
                                    DialogueManager.Instance.TalkStoryStart(lockStartIndex, lockEndIndex, "Table_StoryB1", false);
                                }
                            }
                            else
                            {
                                isTouching = !isTouching;
                                if (PlayerManager.Instance != null)
                                {
                                    PlayerManager.Instance.SetBtn(!isTouching);
                                }
                                if (TouchManager.Instance != null)
                                {
                                    TouchManager.Instance.EnableMoveHandler(!isTouching);
                                }
                                normalCamera.SetActive(isTouching);
                            }
                            break;
                    }
                }
            }
        }
    }
    public void InteractionObject()
    {
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);

        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }
        if (anim != null)
        {
            anim.SetBool(openAnim, true);
        }
    }
    public void OnTouchStarted(Vector2 position)
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

}
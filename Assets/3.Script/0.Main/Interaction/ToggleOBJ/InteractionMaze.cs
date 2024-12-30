using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMaze : TouchPuzzleCanvas
{
    [SerializeField] private PlaneTiltController plane;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (!isInteracted && !UI_InvenManager.Instance.HaveItem(interactionIndex))
                {
                    DialogueManager.Instance.SetDialogue("Table_StoryB1", 17);
                }
                else if (!isInteracted)
                {
                    if (missionStart.activeInHierarchy)
                    {
                        DialogueManager.Instance.SetDialogue("Table_StoryB1", 16);
                    }
                    else
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);
                        outline.enabled = false;
                        if (PlayerManager.Instance != null)
                        {
                            PlayerManager.Instance.SetBtn(false);
                        }
                        UI_InvenManager.Instance.OpenQuickSlot();
                    }
                }
                else
                {
                    if (!missionStart.activeInHierarchy)
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);
                        if (PlayerManager.Instance != null)
                        {
                            PlayerManager.Instance.SetBtn(false);
                        }
                        mask.enabled = false;
                        outline.enabled = false;
                    }
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
        }
    }

    protected override void ClearEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void ResetCamera()
    {
        throw new System.NotImplementedException();
    }
}

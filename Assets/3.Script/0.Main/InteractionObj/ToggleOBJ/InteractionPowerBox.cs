using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPowerBox : TouchPuzzleCanvas
{
    

    public override void OffInteraction()
    {
        base.OffInteraction();
        if (!isClear)
        {
            mask.enabled = true;
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
            missionStart.SetActive(false);
            missionExit.SetActive(false);
            TouchManager.Instance.EnableMoveHandler(true);
        }
        else
        {

            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
            }
            Invoke("ClearEvent", 2f);
        }
    }

    protected override void ClearEvent()
    {
        for (int i = 0; i < interactionAnim.Length; i++)
        {
            interactionAnim[i].SetBool(openAnim, true);
        }
        Invoke("ResetCamera", 1f);
    }
    protected override void ResetCamera()
    {
        missionExit.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        btnList.SetActive(true);
    }

    public override void OnTouchEnd(Vector2 position)
    {
        base.OnTouchEnd(position);
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
                    if (missionStart.activeInHierarchy)
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
                    mask.enabled = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (isClear) return;

        if (interactionCam != null)
        {
            if (!interactionCam.activeInHierarchy)
            {
                if (other.CompareTag("MainCamera") && outline != null)
                {
                    outline.enabled = true;
                }
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (isClear)
        {
            outline.enabled = false;
            return;
        }

        if (interactionCam != null)
        {
            if (interactionCam.activeInHierarchy && outline.enabled)
            {
                outline.enabled = false;
            }
            else
            {
                outline.enabled = true;
            }
        }
    }

    
}

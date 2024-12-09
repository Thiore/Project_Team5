using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSlidePuzzle : TouchPuzzleCanvas
{
    private bool isOpen;

    private void Awake()
    {
        isOpen = false;
    }
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
        throw new System.NotImplementedException();
    }

    protected override void ResetCamera()
    {
        throw new System.NotImplementedException();
    }

    
}

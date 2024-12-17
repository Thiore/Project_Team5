using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBattery : TouchPuzzleCanvas
{
    [SerializeField] private Battery[] batterys;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(!isClear)
        {
            foreach(var battery in batterys)
            {
                battery.CheckBattery += CheckConnecting;
                battery.isStart = false;
            }
        }
    }
    public override void OffInteraction()
    {
        base.OffInteraction();

        mask.enabled = true;
        missionStart.SetActive(false);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
        foreach (var battery in batterys)
        {
            battery.isStart = false;
        }
    }
    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if(hit.collider.gameObject.Equals(gameObject))
            {
                if (!missionStart.activeInHierarchy)
                {
                    missionStart.SetActive(true);
                    btnExit.SetActive(true);
                    TouchManager.Instance.EnableMoveHandler(false);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(false);
                    }
                    mask.enabled = false;
                    foreach (var battery in batterys)
                    {
                        battery.isStart = true;
                    }
                }

                if(!isInteracted)
                    UI_InvenManager.Instance.OpenQuickSlot();
            }
        }
    }

    protected override void ClearEvent()
    {
        
    }

    protected override void ResetCamera()
    {
        
    }

    private void CheckConnecting()
    {
        foreach(var battery in batterys)
        {
            if(!battery.isRed||!battery.isBlack)
            {
                return;
            }
        }
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
        isClear = true;
        OffInteraction();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (isClear) return;
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }
}

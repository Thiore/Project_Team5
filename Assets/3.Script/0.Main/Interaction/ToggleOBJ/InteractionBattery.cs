using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBattery : TouchPuzzleCanvas
{
    [SerializeField] private Battery[] batterys;

    //성공한 이후 게임을 불러왔을때 보여주기 위해 사용
    [SerializeField] private Connection[] clearRedConnect;
    [SerializeField] private Connection[] clearBlackConnect;

    protected override void Start()
    {
        if (!isClear)
        {
            base.Start();

            foreach (var battery in batterys)
            {
                battery.CheckBattery += CheckConnecting;
                battery.isStart = false;
            }


        }
        else
        {
            FinishBattery();
        }
    }
    public override void OffInteraction()
    {
        base.OffInteraction();

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
        if (!isClear)
        {
            mask.enabled = true;
            outline.enabled = true;
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
                if (!isInteracted)
                {
                    if (UI_InvenManager.Instance.HaveItem(interactionIndex))
                        UI_InvenManager.Instance.OpenQuickSlot();
                    else
                    {
                        DialogueManager.Instance.SetDialogue("Table_StoryB1", 24);
                        return;
                    }
                }
                else
                {
                    foreach (var battery in batterys)
                    {
                        battery.isStart = true;
                    }
                }
                if (!missionStart.activeInHierarchy)
                {
                    mask.enabled = false;
                    missionStart.SetActive(true);
                    btnExit.SetActive(true);
                    TouchManager.Instance.EnableMoveHandler(false);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(false);
                    }
                    outline.enabled = false;
                    
                }
            }
        }
    }
    
    public override void InteractionObject(int id)
    {
        base.InteractionObject(id);
        if(isInteracted)
        {
            foreach (var battery in batterys)
            {
                battery.isStart = true;
            }
            DialogueManager.Instance.SetDialogue("Table_StoryB1", 25);
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

    private void FinishBattery()
    {
        batterys[5].transform.rotation = Quaternion.identity;
        batterys[3].transform.eulerAngles = new Vector3(0f, 180f, 0f);
        batterys[2].transform.eulerAngles = new Vector3(0f, 90f, 0f);

        for(int i = 0; i < clearRedConnect.Length;++i)
        {
            clearRedConnect[i].line.enabled = true;
            clearRedConnect[i].line.SetPosition(0, clearRedConnect[i].transform.position);
            clearRedConnect[i].line.SetPosition(1, clearBlackConnect[i].transform.position);
        }
    }
}

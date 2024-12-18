using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionFuseBox : TouchPuzzleCanvas
{
    [SerializeField] private Fuse[] resultfuselist;
    [SerializeField] private Fuse_Binker[] blinkers;
    [SerializeField] private Image[] fuseimages;

    [SerializeField] private GameObject clearObj;

    [SerializeField] private Outline resetBtnOutline;

    [SerializeField] private Outline[] emptyFuses;

    private int[] result = new int[] { 0, 2, 1, 0, 1, 2 };

    private void Awake()
    {
        foreach (Outline outline in emptyFuses)
        {
            outline.enabled = false;
        }
    }

    public void CheckResult()
    {
        Debug.Log(resultfuselist.Length);
        Debug.Log(result.Length);
        int samecount = 0;


        for (int i = 0; i < resultfuselist.Length; i++)
        {
            //비었을때 누른다면
            if (i < 4 && !resultfuselist[i].gameObject.activeSelf)
            {
                blinkers[i].SetBlinkerColor(0);
            }

            // 비교와 같다면
            if (i < 4 && resultfuselist[i].gameObject.activeSelf)
            {
                if (resultfuselist[i].GetFuseColor().Equals(result[i]))
                {
                    blinkers[i].SetBlinkerColor(1);
                    samecount++;
                }
                else
                {
                    blinkers[i].SetBlinkerColor(0);
                }
            }
        }

        if (resultfuselist[4].gameObject.activeSelf && resultfuselist[5].gameObject.activeSelf)
        {
            if (resultfuselist[4].GetFuseColor().Equals(result[4]) && resultfuselist[5].GetFuseColor().Equals(result[5]))
            {
                blinkers[4].SetBlinkerColor(1);
                samecount++;
            }
            else
            {
                blinkers[4].SetBlinkerColor(0);
            }
        }
        else
        {
            blinkers[4].SetBlinkerColor(0);
        }


        if (samecount.Equals(5))
        {
            DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
            OffInteraction();
        }
        else
        {
            FuseReset();
        }
    }

    public void FuseReset()
    {

        for (int i = 0; i < resultfuselist.Length; i++)
        {
            if (resultfuselist[i].gameObject.activeSelf)
            {
                resultfuselist[i].SetFuseColor(3);
                resultfuselist[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < fuseimages.Length; i++)
        {
            if (fuseimages[i].enabled.Equals(false))
            {
                fuseimages[i].enabled = true;
            }
        }
    }

    public override void OffInteraction()
    {
        base.OffInteraction();
        resetBtnOutline.enabled = false;
        foreach (Outline outline in emptyFuses)
        {
            outline.enabled = false;
        }
        if (!isClear)
        {
            mask.enabled = true;
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
            
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.SetBtn(true);
            }
            TouchManager.Instance.EnableMoveHandler(true);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
            }
            missionStart.SetActive(false);

        }
        else
        {
            clearObj.SetActive(false);
            
            Invoke("ClearEvent", 2f);
        }
    }

    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;
        if (!isInteracted)
        {
            isInteracted = DataSaveManager.Instance.GetGameState(floorIndex, interactionIndex[0]);
        }
        if (isInteracted)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    TouchManager.Instance.EnableMoveHandler(false);
                    missionStart.SetActive(true);
                    btnExit.SetActive(true);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(false);
                    }
                    if (anim != null)
                    {
                        anim.SetBool(openAnim, true);

                    }
                    mask.enabled = false;
                    outline.enabled = false;
                    resetBtnOutline.enabled = true;
                }

            }
        }

    }

    protected override void ClearEvent()
    {
        if (UI_InvenManager.Instance.isOpenQuick)
        {
            UI_InvenManager.Instance.CloseQuickSlot();
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
        missionStart.SetActive(false);
        if (anim != null)
        {
            anim.SetBool(openAnim, false);
        }
    }

    protected override void ResetCamera()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isClear) return;


        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }
    

}
    


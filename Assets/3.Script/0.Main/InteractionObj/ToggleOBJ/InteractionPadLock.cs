using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPadLock : TouchPuzzleCanvas
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
            missionStart.SetActive(false);
            missionExit.SetActive(false);
            TouchManager.Instance.EnableMoveHandler(true);
        }
        else
        {
            missionStart.SetActive(false);
            Invoke("ClearEvent", 2f);
        }
    }
    protected override void ClearEvent()
    {
        if (anim != null)
        {
            anim.SetBool(openAnim, true);
        }
        Invoke("ResetCamera", 1f);
    }
    protected override void ResetCamera()
    {
        mask.enabled = true;
        missionExit.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        btnList.SetActive(true);
    }
    public override void OnTouchEnd(Vector2 position)
    {
        if(!isClear)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    TouchManager.Instance.EnableMoveHandler(false);
                    missionStart.SetActive(true);
                    missionExit.SetActive(true);
                    btnExit.SetActive(true);
                    mask.enabled = false;

                   

                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    if (anim != null)
                    {
                        isOpen = !isOpen;
                        TouchManager.Instance.EnableMoveHandler(!isOpen);
                        interactionCam.SetActive(isOpen);
                        anim.SetBool(openAnim, isOpen);
                    }
                }
            }
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }

    }

}

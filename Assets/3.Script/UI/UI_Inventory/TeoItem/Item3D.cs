using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item3D : TeoItem, ITouchable
{
    private TeoLerp lerpImage;
    private Inventory inven;

    private Outline outline;
    [SerializeField] private GameObject cam;

    protected override void OnEnable()
    {
        base.OnEnable();
        lerpImage = PlayerManager.Instance.getLerpImage;
        inven = PlayerManager.Instance.getInven;
    }
    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                Debug.Log(itemData.sprite.name);
                if(lerpImage.InputMovementInventory(itemData, position))
                {

                    gameObject.SetActive(false);
                }

            }
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        return;
    }

    public void OnTouchStarted(Vector2 position)
    {
        return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (cam != null)
        {
            if (cam.activeSelf)
            {
                if (outline != null && other.CompareTag("MainCamera"))
                {
                    outline.enabled = true;
                }
            }

        }
        else
        {
            if (outline != null && other.CompareTag("MainCamera"))
            {
                outline.enabled = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (outline != null && other.CompareTag("MainCamera"))
        {
            outline.enabled = false;
        }
    }

}

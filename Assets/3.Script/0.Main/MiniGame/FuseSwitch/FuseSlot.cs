using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FuseSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private int efuseNum;
    [SerializeField] private Fuse dragfuse;
    [SerializeField] private Image image;
    public Image Image { get => image; }
    private bool isdragging;
    private Vector3 tartgetpos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragfuse.gameObject.SetActive(true);
        dragfuse.SetFuseColor(efuseNum);
        image.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isdragging = true;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask("SlideWall")))
        {
            tartgetpos = hit.point;
            dragfuse.gameObject.transform.position = tartgetpos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, ~(LayerMask.GetMask("SlideWall"))))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.name.Equals("EmptyFuse"))
            {
                if (hit.transform.GetChild(0).TryGetComponent(out ResultFuse fuse))
                {
                    if (!hit.transform.GetChild(0).gameObject.activeSelf)
                    {
                        hit.transform.GetChild(0).gameObject.SetActive(true);
                        fuse.SetFuseColor(efuseNum);
                        fuse.GetSlot(this);
                    }
                    else
                    {
                        image.enabled = true;
                    }
                }
            }
            else
            {
                image.enabled = true;
            }
        }
        else
        {
            image.enabled = true;
        }

        dragfuse.gameObject.SetActive(false);
        isdragging = false;
    }


    public FuseSlot ResultSaveSlot()
    {
        return this;
    }

}

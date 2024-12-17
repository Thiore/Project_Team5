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
    private bool isDragging;
    private Vector3 tartgetpos;

    private void Awake()
    {
        isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            dragfuse.gameObject.SetActive(true);
            dragfuse.SetFuseColor(efuseNum);
            image.enabled = false;
            isDragging = true;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask("Area")))
            {
                tartgetpos = hit.point;
                dragfuse.gameObject.transform.position = tartgetpos;
            }
            else
            {
                image.enabled = true;
                dragfuse.gameObject.SetActive(false);
                isDragging = false;
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 50f, ~(LayerMask.GetMask("Area"))))
            {
                if (hit.collider.CompareTag("EmptyFuse"))
                {
                    if (hit.transform.GetChild(0).TryGetComponent(out ResultFuse fuse))
                    {
                        if (!hit.transform.GetChild(0).gameObject.activeInHierarchy)
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
            isDragging = false;
        }
        
    }


    public FuseSlot ResultSaveSlot()
    {
        return this;
    }

}

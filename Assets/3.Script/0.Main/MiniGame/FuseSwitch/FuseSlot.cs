using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuseSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Fuse> fuselist;
    [SerializeField] private int efuseNum;
    private GameObject ondragobj;
    private bool isdragging;
    private Vector3 tartgetpos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        for (int i = 0; i < fuselist.Count; i++)
        {
            if (fuselist[i].gameObject.activeSelf)
            {
                ondragobj = fuselist[i].gameObject;
                break;
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        isdragging = true;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, LayerMask.GetMask("SlideWall")))
        {
            Debug.Log("검출됨");
            tartgetpos = hit.point;
            ondragobj.transform.position = tartgetpos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        RaycastHit[] hits = Physics.RaycastAll(tartgetpos, Vector3.forward);

        Debug.Log(hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.name.Equals("EmptyFuse"))
            {
                if (hits[i].transform.TryGetComponent(out Fuse fuse))
                {
                    Debug.Log("검출됨");
                    fuse.SetFuseColor(efuseNum);
                    break;
                }
            }
        }

        ondragobj.SetActive(false);
        ondragobj = null;
        isdragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }



}

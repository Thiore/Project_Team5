using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FuseSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Fuse> fuselist;
    [SerializeField] private int efuseNum;
    [SerializeField] private Fuse dragfuse;
    private bool isdragging;
    private Vector3 tartgetpos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragfuse.gameObject.SetActive(true);
        dragfuse.SetFuseColor(efuseNum);
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

        dragfuse.gameObject.SetActive(false);
        isdragging = false;
    }


}

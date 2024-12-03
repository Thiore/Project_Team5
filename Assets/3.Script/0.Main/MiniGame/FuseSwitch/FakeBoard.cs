using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FakeBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastHit[] hits = Physics.RaycastAll(eventData.position, Vector3.forward);
        Debug.Log($"엠티퓨즈 {hits.Length}");
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.name.Equals("EmptyFuse"))
            {
                if (hits[i].transform.TryGetComponent(out EmptyFuse fuse))
                {
                    Debug.Log("초기화");
                    fuse.SetFuseColor(3);
                    break;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

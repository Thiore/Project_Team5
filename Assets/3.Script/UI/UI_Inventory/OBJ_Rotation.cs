using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OBJ_Rotation : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform obj;

    

    private void OnEnable()
    {
        obj.rotation = Quaternion.identity;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 angle = new Vector3(eventData.scrollDelta.y, eventData.scrollDelta.x, 0f);
        obj.Rotate(angle);
    }

}

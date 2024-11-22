using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OBJ_Rotation : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform obj;

    private Vector2 startValue;
    private Vector2 value;
    private bool isDrag = false;

    private void OnEnable()
    {
        obj.rotation = Quaternion.identity;
        startValue = Vector2.zero;
        value = Vector2.zero;
    }
    private void Update()
    {
        if(!isDrag)
        {
            startValue = Vector2.zero;
            value = Vector2.zero;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(startValue.Equals(Vector2.zero))
        {
            startValue = eventData.position;
            
        }
        value = eventData.position;
        Vector2 delta = startValue - value;
        Vector3 angle = new Vector3(delta.y, delta.x, 0f);
        obj.Rotate(angle);
        startValue = value;
    }

}

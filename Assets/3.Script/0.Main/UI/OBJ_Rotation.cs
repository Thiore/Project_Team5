using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class OBJ_Rotation : MonoBehaviour
{
    [SerializeField] private InputAction FingerAction;
    

    [SerializeField] private Transform obj;

    //private Vector2 startValue;
    //private Vector2 value;
    //private bool isDrag = false;

    private bool isTwoFinger;
    private Vector2 startOneFingerValue;
    private Vector2 oneFingerValue;
    private Vector2 startTwoFingerValue;
    private Vector2 twoFingerValue;
    private float startFingersDistance;

    

    private void OnEnable()
    {
        FingerAction.performed += OnFingerPerformed;
        FingerAction.Enable();
        isTwoFinger = true;
        obj.rotation = Quaternion.identity;
        startOneFingerValue = Vector2.zero;
        oneFingerValue = Vector2.zero;
        startTwoFingerValue = Vector2.zero;
        twoFingerValue = Vector2.zero;
        startFingersDistance = 0f;
        //startValue = Vector2.zero;
        //value = Vector2.zero;
    }
    private void OnDisable()
    {
        FingerAction.performed -= OnFingerPerformed;
        FingerAction.Disable();
    }
    //private void Update()
    //{
    //    
    //    
    //    
    //    
    //    //if (!isDrag)
    //    //{
    //    //    startValue = Vector2.zero;
    //    //    value = Vector2.zero;
    //    //}
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    if(startValue.Equals(Vector2.zero))
    //    {
    //        startValue = eventData.position;

    //    }
    //    value = eventData.position;
    //    Vector2 delta = startValue - value;
    //    Vector3 angle = new Vector3(delta.y, delta.x, 0f);
    //    obj.Rotate(angle);
    //    startValue = value;
    //}
   
    
    private void OnFingerPerformed(InputAction.CallbackContext context)
    {
        var touches = Touchscreen.current.touches;
        if(touches.Count>=2&&!isTwoFinger)
        {
            isTwoFinger = true;
            foreach(var touch in touches)
            {
                if(startOneFingerValue.Equals(Vector2.zero))
                {
                    startOneFingerValue = touch.position.ReadValue();
                    continue;
                }
                else
                {
                    startTwoFingerValue = touch.position.ReadValue();
                    startFingersDistance = Vector2.Distance(startOneFingerValue, startTwoFingerValue);
                    return;
                }
            }
        }
        else if(touches.Count.Equals(1)&&isTwoFinger)
        {
            isTwoFinger = false;
            startOneFingerValue = context.ReadValue<Vector2>();
            oneFingerValue = context.ReadValue<Vector2>();
            return;
        }

        if (isTwoFinger)
        {
            oneFingerValue = touches[0].position.ReadValue();
            twoFingerValue = touches[1].position.ReadValue();
            float fingerDistance = Vector2.Distance(oneFingerValue, twoFingerValue);
            
            if (fingerDistance < 100f) return;

            float distance = fingerDistance - startFingersDistance;

            obj.localScale = new Vector3(1f, 1f, 1f) * distance * 0.1f;
        }
        else
        {
            oneFingerValue = context.ReadValue<Vector2>();
            Vector2 delta = oneFingerValue - startOneFingerValue;
            obj.Rotate(Vector3.up, delta.x);
            obj.Rotate(Vector3.right, delta.y);
            startOneFingerValue = oneFingerValue;
        }
    }
    
}

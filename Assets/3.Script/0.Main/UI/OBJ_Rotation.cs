using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class OBJ_Rotation : MonoBehaviour
{
    [SerializeField] private InputAction oneFingerAction;
    [SerializeField] private InputAction twoFingerAction;
    

    [SerializeField] private Transform obj;


    private bool isOneFinger;
    private bool isTwoFinger;
    private Vector2 startOneFingerValue;
    private Vector2 oneFingerValue;

    private Vector2 firstFingerStartValue;
    private Vector2 secondFingerStartValue;
    
    private float startFingersDistance;

    

    private void OnEnable()
    {
        oneFingerAction.started += OnOneFingerStarted;
        oneFingerAction.performed += OnOneFingerPerformed;
        oneFingerAction.canceled += OnOneFingerCanceled;
        oneFingerAction.Enable();
        twoFingerAction.started += OnTwoFingerStarted;
        twoFingerAction.performed += OnTwoFingerPerformed;
        twoFingerAction.canceled += OnTwoFingerCanceled;
        twoFingerAction.Enable();
        isOneFinger = false;
        isTwoFinger = false;
        obj.localScale = Vector3.one;
        //startValue = Vector2.zero;
        //value = Vector2.zero;
    }
    private void OnDisable()
    {
        oneFingerAction.started -= OnOneFingerStarted;
        oneFingerAction.performed -= OnOneFingerPerformed;
        oneFingerAction.canceled -= OnOneFingerCanceled;
        oneFingerAction.Disable();
        twoFingerAction.started -= OnTwoFingerStarted;
        twoFingerAction.performed -= OnTwoFingerPerformed;
        twoFingerAction.canceled -= OnTwoFingerCanceled;
        twoFingerAction.Disable();
    }
   private void OnOneFingerStarted(InputAction.CallbackContext context)
   {
        if(!isTwoFinger)
        {
            isOneFinger = true;
            startOneFingerValue = context.ReadValue<Vector2>();
            oneFingerValue = context.ReadValue<Vector2>();
            TouchManager.Instance.EnableMoveHandler(false);
        }
   }
    

    private void OnOneFingerPerformed(InputAction.CallbackContext context)
    {
        if (!isTwoFinger&&isOneFinger)
        {
            
                oneFingerValue = context.ReadValue<Vector2>();
                Vector2 delta = oneFingerValue - startOneFingerValue;
                obj.Rotate(Vector3.up, delta.x * 0.3f);
                obj.Rotate(Vector3.right, -delta.y*0.3f);
                startOneFingerValue = oneFingerValue;
             
        }
    }
    private void OnOneFingerCanceled(InputAction.CallbackContext context)
    {
            isOneFinger = false;
            TouchManager.Instance.EnableMoveHandler(true);
        
    }

    private void OnTwoFingerStarted(InputAction.CallbackContext context)
    {
        if (!isTwoFinger)
        {
            isTwoFinger = true;
            var touches = Touchscreen.current.touches;
            Debug.Log(Touchscreen.current.touches.Count);

            firstFingerStartValue = touches[0].position.ReadValue();
            secondFingerStartValue = touches[1].position.ReadValue();

            startFingersDistance = Vector2.Distance(firstFingerStartValue, secondFingerStartValue);

        }
    }
    private void OnTwoFingerPerformed(InputAction.CallbackContext context)
    {
        if (isTwoFinger)
        {
            var touches = Touchscreen.current.touches;
            firstFingerStartValue = touches[0].position.ReadValue();
            secondFingerStartValue = touches[1].position.ReadValue();
            float fingerDistance = Vector2.Distance(firstFingerStartValue, secondFingerStartValue);

            float scaleChange = (fingerDistance - startFingersDistance) * 0.02f;
            obj.localScale += Vector3.one * scaleChange;
            obj.localScale = Vector3.Max(obj.localScale, Vector3.one * 1f); // 최소 크기 제한
            obj.localScale = Vector3.Min(obj.localScale, Vector3.one * 5f);   // 최대 크기 제한

            startFingersDistance = fingerDistance;
        }
    }
    private void OnTwoFingerCanceled(InputAction.CallbackContext context)
    {
            isTwoFinger = false;
        
    }

}

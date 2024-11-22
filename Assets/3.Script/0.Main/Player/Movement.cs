 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class Movement : MonoBehaviour, ITouchable
{ 
    
    [SerializeField] private Transform playerCamera; 

    
    [SerializeField] private float speed;

    private Vector2 startValue;
    private Vector2 value;
    
    Vector3 moveDir;
    

    private void Start()
    {
        TouchManager.Instance.OnMoveStarted += OnTouchStarted;
        TouchManager.Instance.OnMoveHold += OnTouchHold;
        TouchManager.Instance.OnMoveEnd += OnTouchEnd;
        moveDir = Vector3.zero;
        startValue = Vector2.zero;
        value = Vector2.zero;

        SaveManager.Instance.LoadPlayerPosition(transform);
        SaveManager.Instance.LoadPlayerRotation(playerCamera.transform);
        
        playerCamera.transform.localPosition = transform.localPosition + Vector3.up * 0.5f;
    }

    private void FixedUpdate()
    {
        if(!moveDir.Equals(Vector3.zero))
        {
            transform.Translate(moveDir * speed * Time.unscaledDeltaTime);
            playerCamera.transform.localPosition = transform.localPosition + Vector3.up * 0.5f;
        }
        
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        startValue = position;
        value = position;
    }

    public void OnTouchHold(Vector2 position)
    {
        Vector3 cameraFoward = playerCamera.transform.forward;
        cameraFoward.y = 0f;
        cameraFoward = cameraFoward.normalized;

        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight = cameraRight.normalized;

        value = position;
        Vector2 joystickInput = value - startValue;
        joystickInput = joystickInput.normalized;
        moveDir = (cameraRight * joystickInput.x + cameraFoward * joystickInput.y).normalized;
    }

    public void OnTouchEnd(Vector2 position)
    {
        startValue = Vector2.zero;
        value = Vector2.zero;
        moveDir = Vector3.zero;        
    }
}

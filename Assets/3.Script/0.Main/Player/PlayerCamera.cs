using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, ITouchable
{
    //[SerializeField] private Slider slideSpeed;
    private Vector2 lastTouchPosition;

    private Vector3 deltaRot;
    [SerializeField] private float clampRotX;

    private float currentRotationX;

    private void OnEnable()
    {
        if (GameManager.Instance.gameType.Equals(eGameType.LoadGame))
        {
            transform.rotation = DataSaveManager.Instance.LoadGamePlayerRotation();
        }
        else
        {
            transform.rotation = DataSaveManager.Instance.NewGamePlayerRotation();
        }
        lastTouchPosition = Vector2.zero;
        deltaRot = Vector3.zero;
        currentRotationX = transform.localEulerAngles.x; // 누적 X축 회전값을 추적하는 변수

        TouchManager.Instance.OnLookStarted += OnTouchStarted;
        TouchManager.Instance.OnLookHold += OnTouchHold;
        TouchManager.Instance.OnLookEnd += OnTouchEnd;
        
    }
    private void OnDisable()
    {
        TouchManager.Instance.OnMoveStarted -= OnTouchStarted;
        TouchManager.Instance.OnMoveHold -= OnTouchHold;
        TouchManager.Instance.OnMoveEnd -= OnTouchEnd;
    }

    private void FixedUpdate()
    {
        // 터치 delta 값을 기반으로 X축 회전값을 갱신
        currentRotationX -= deltaRot.y * Time.fixedDeltaTime * 5f;
        currentRotationX = Mathf.Clamp(currentRotationX, -clampRotX, clampRotX); // X축 회전 제한

        // Y축 회전 적용 (deltaRot.x는 Y축 회전으로 사용)
        float rotationY = deltaRot.x * Time.fixedDeltaTime * 5f;

        // transform.localEulerAngles에 제한된 회전값을 설정
        transform.localEulerAngles = new Vector3(currentRotationX, transform.localEulerAngles.y + rotationY, 0f);

        // deltaRot 초기화
        deltaRot = Vector3.zero;
    }


    

    public void OnTouchStarted(Vector2 position)
    {
        lastTouchPosition = position;
    }

    public void OnTouchHold(Vector2 position)
    {
        if (!position.Equals(lastTouchPosition))
        {
            Vector2 delta = position - lastTouchPosition;
            //빼야함
            if(SettingsManager.Instance == null)
            {
                deltaRot = new Vector3(delta.x * 3f,
                                  delta.y * 3f);
            }
            else
            {
                deltaRot = new Vector3(delta.x * SettingsManager.Instance.CameraSpeed,
                                   delta.y * SettingsManager.Instance.CameraSpeed);
            }
            

            lastTouchPosition = position;
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        deltaRot = Vector3.zero;
        lastTouchPosition = Vector2.zero;
    }
}
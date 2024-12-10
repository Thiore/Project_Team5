using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour, ITouchable
{
    [SerializeField] private RectTransform joystick;
    [SerializeField] private RectTransform handle;

    private RectTransform panel;
    //private Canvas canvas;
    

    private Vector2 joystickCenter; // 조이스틱 중심 좌표
    public float maxDistance = 70f; // 핸들이 움직일 수 있는 최대 거리


    private void Awake()
    {
        TryGetComponent(out panel);
        TouchManager.Instance.OnMoveStarted += OnTouchStarted;
        TouchManager.Instance.OnMoveHold += OnTouchHold;
        TouchManager.Instance.OnMoveEnd += OnTouchEnd;

    }

    private void OnDestroy()
    {
        TouchManager.Instance.OnMoveStarted -= OnTouchStarted;
        TouchManager.Instance.OnMoveHold -= OnTouchHold;
        TouchManager.Instance.OnMoveEnd -= OnTouchEnd;
    }




    /// <summary>
    /// 매 프레임 터치 위치에 따라서 가상 조이패드 핸들의 좌표 지정
    /// </summary>
    /// <param name="현재 터치되고 있는 위치 좌표"></param>
    private void MoveHandle(Vector2 touchPosition)
    {
        // 터치된 스크린 좌표를 조이스틱 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystick,
            touchPosition,
            null,
            out localPoint
        );

        // 핸들이 조이스틱 중심에서 벗어난 거리를 계산
        Vector2 offset = localPoint;
        
        // 핸들이 최대 80f 거리 이상으로 이동하지 않도록 제한
        if (offset.magnitude > maxDistance)
        {
            offset = offset.normalized * maxDistance; // 방향은 유지하되, 거리는 80f로 제한
        }
        // 핸들의 위치를 업데이트 (조이스틱 중심에서 offset만큼 이동)
        handle.anchoredPosition = offset;
    }

    /// <summary>
    /// 조이스틱 활성화/비활성화
    /// </summary>
    /// <param name="touchPosition">가상조이패드가 활성화될 터치 위치</param>
    /// <param name="isActive">가상조이패드의 활성화 여부</param>
    private void ActivateJoystick(Vector2 touchPosition, bool isActive)
    {
        if (isActive)
        {
            // 터치된 위치를 캔버스 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panel,
                touchPosition,
                null,
                out localPoint
            );

            // 조이스틱을 터치된 위치에 배치
            joystick.anchoredPosition = localPoint; 
           
            joystickCenter = localPoint; // 조이스틱 중심 좌표 저장
            joystick.gameObject.SetActive(true);
        }
        else
        {
            joystick.gameObject.SetActive(false); // 조이스틱 비활성화
        }
    }

    public void OnTouchStarted(Vector2 position)
    {
        ActivateJoystick(position, true); // 조이스틱 활성화
        joystickCenter = joystick.anchoredPosition; // 조이스틱 중심 좌표 저장
    }
    
    public void OnTouchHold(Vector2 position)
    {
        // 현재 터치된 위치를 가져와서 조이스틱 중심에서 핸들을 이동시킴
        MoveHandle(position); // 터치 위치에 따른 핸들 이동
    }

    public void OnTouchEnd(Vector2 position)
    {
        handle.anchoredPosition = Vector2.zero; // 핸들을 중앙으로 복귀
        ActivateJoystick(Vector2.zero, false); // 조이스틱 비활성화
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickManager : MonoBehaviour
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject handle;

    //private InputManager input;

    private Canvas canvas;  // 캔버스 참조
    private Vector2 joystickCenter; // 조이스틱 중심 좌표
    public float maxDistance = 70f; // 핸들이 움직일 수 있는 최대 거리
    private bool joystickActive; // 조이스틱 활성화 여부

    private void Start()
    {
        //Input Data가져오기
        //input = InputManager.Instance;
        // 캔버스 참조 가져오기
        TryGetComponent(out canvas);
        joystickActive = false;
    }

    private void Update()
    {
        //if(input.moveData.isTouch)
        //{
        //    // 터치 상태에 따라 처리
        //    if (joystickActive)
        //    {
        //        HandleMove();
        //    }
        //    else
        //    {
        //        HandleTouchBegan(input.moveData.startValue);
        //    }
        //}
        //else
        //{
        //    if(joystickActive)
        //        HandleTouchEnded();
        //}
    }

    /// <summary>
    /// 터치 시작 시 호출
    /// </summary>
    /// <param name="touchPosition">조이스틱을 활성화 할 터치 시작 좌표</param>
    private void HandleTouchBegan(Vector2 touchPosition)
    {
        ActivateJoystick(touchPosition, true); // 조이스틱 활성화
        joystick.TryGetComponent(out RectTransform rect);
        joystickCenter = rect.anchoredPosition; // 조이스틱 중심 좌표 저장
        joystickActive = true; // 조이스틱 활성화 플래그 설정
    }

    /// <summary>
    /// 터치 이동 시 호출(손가락 이동에 따라 핸들이 바로 반응)
    /// </summary>
    private void HandleMove()
    {
            // 현재 터치된 위치를 가져와서 조이스틱 중심에서 핸들을 이동시킴
            //MoveHandle(input.moveData.value); // 터치 위치에 따른 핸들 이동
    }

    /// <summary>
    /// 매 프레임 터치 위치에 따라서 가상 조이패드 핸들의 좌표 지정
    /// </summary>
    /// <param name="touchPosition"></param>
    private void MoveHandle(Vector2 touchPosition)
    {
        // 터치된 스크린 좌표를 조이스틱 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystick.GetComponent<RectTransform>(),
            touchPosition,
            canvas.worldCamera,
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
        handle.GetComponent<RectTransform>().anchoredPosition = offset;
    }

    /// <summary>
    /// 터치 종료시 호출 및 초기화
    /// </summary>
    private void HandleTouchEnded()
    {
            handle.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 핸들을 중앙으로 복귀
            ActivateJoystick(Vector2.zero, false); // 조이스틱 비활성화
            joystickActive = false; // 조이스틱 활성화 플래그 해제
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
                (RectTransform)canvas.transform,
                touchPosition,
                canvas.worldCamera,
                out localPoint
            );

            // 조이스틱을 터치된 위치에 배치
            joystick.GetComponent<RectTransform>().anchoredPosition = localPoint; 
           
            joystickCenter = localPoint; // 조이스틱 중심 좌표 저장
            joystick.SetActive(true);
            joystickActive = true;
        }
        else
        {
            joystick.SetActive(false); // 조이스틱 비활성화
            joystickActive = false;
        }
    }
}

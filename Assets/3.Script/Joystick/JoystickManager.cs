using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickManager : MonoBehaviour
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private GameObject handle;
    [SerializeField] private RectTransform leftArea;
    [SerializeField] private RectTransform joystickArea;

    public LayerMask touchableLayer;
    private Canvas canvas;  // 캔버스 참조
    private Vector2 joystickCenter; // 조이스틱 중심 좌표
    public float maxDistance = 70f; // 핸들이 움직일 수 있는 최대 거리
    private bool joystickActive; // 조이스틱 활성화 여부

    private void Start()
    {
        // 캔버스 참조 가져오기
        canvas = GetComponentInParent<Canvas>();
        joystickActive = false;
        // 조이스틱을 초기에는 비활성화
        joystick.SetActive(false);
    }

    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // 터치 상태에 따라 처리
            if (joystickActive)
            {
                HandleMove();
            }
            else
            {
                HandleTouchBegan(touchPosition);
            }
        }

        HandleTouchEnded();
    }

    // 터치 시작 시 호출
    private void HandleTouchBegan(Vector2 touchPosition)
    {
        if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition, canvas.worldCamera))
            {
                ActivateJoystick(touchPosition, true); // 조이스틱 활성화
                joystickCenter = joystick.GetComponent<RectTransform>().anchoredPosition; // 조이스틱 중심 좌표 저장
                joystickActive = true; // 조이스틱 활성화 플래그 설정
            }else if (RectTransformUtility.RectangleContainsScreenPoint(leftArea, touchPosition, canvas.worldCamera))
            {
                HandleLeftAreaTouch(touchPosition);
            }
        }
    }

    // 터치 이동 시 호출 (손가락 이동에 따라 핸들이 바로 반응)
    private void HandleMove()
    {
            // 현재 터치된 위치를 가져와서 조이스틱 중심에서 핸들을 이동시킴
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            MoveHandle(touchPosition); // 터치 위치에 따른 핸들 이동
    }

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
        float maxDistance = 80f;
        if (offset.magnitude > maxDistance)
        {
            offset = offset.normalized * maxDistance; // 방향은 유지하되, 거리는 80f로 제한
        }
        Debug.Log($"offset : {offset}");
        // 핸들의 위치를 업데이트 (조이스틱 중심에서 offset만큼 이동)
        handle.GetComponent<RectTransform>().anchoredPosition = offset;
    }

    // 터치가 종료되었을 때 호출
    private void HandleTouchEnded()
    {
        if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            handle.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 핸들을 중앙으로 복귀
            ActivateJoystick(Vector2.zero, false); // 조이스틱 비활성화
            joystickActive = false; // 조이스틱 활성화 플래그 해제
        }
    }

    // LeftArea 터치 처리
    private void HandleLeftAreaTouch(Vector2 touchPosition)
    {
        // 터치된 위치에 "Touchable Object" 태그를 가진 오브젝트가 있는지 확인
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableLayer))
        {
            if (hit.collider.CompareTag("touchableobject"))
            {
                return;
            }
        }

        // 조이스틱 활성화
        ActivateJoystick(touchPosition, true);
    }

    // 조이스틱 활성화/비활성화
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

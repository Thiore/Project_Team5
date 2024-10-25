using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickManager : MonoBehaviour
{
    [SerializeField] private GameObject joystick;
    [SerializeField] private RectTransform leftArea;
    [SerializeField] private RectTransform joystickArea;

    public LayerMask touchableLayer;
    private Canvas canvas;  // 캔버스 참조

    private void Start()
    {
        // 캔버스 찾기 (부모에서 캔버스 컴포넌트를 찾습니다)
        canvas = GetComponentInParent<Canvas>();

        // 조이스틱을 초기에는 비활성화
        joystick.SetActive(false);
    }

    private void Update()
    {
        // 터치 입력이 있을 때 처리
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // 터치가 시작되었을 때
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                // LeftArea를 터치했는지 확인
                if (RectTransformUtility.RectangleContainsScreenPoint(leftArea, touchPosition, canvas.worldCamera))
                {
                    HandleLeftAreaTouch(touchPosition);
                }
                // JoystickArea를 터치했는지 확인
                else if (RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition, canvas.worldCamera))
                {
                    ActivateJoystick(touchPosition); // 바로 조이스틱 활성화
                }
            }

            // 터치가 끝났을 때 조이스틱 비활성화
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                ActivateJoystick(Vector2.zero, false); // 조이스틱 비활성화
            }
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
            // "Touchable Object" 태그를 가진 오브젝트가 있으면 아무 동작도 하지 않음
            if (hit.collider.CompareTag("Touchable Object"))
            {
                return;
            }
        }

        // 없으면 조이스틱 활성화
        ActivateJoystick(touchPosition, true);
    }

    // 조이스틱 활성화/비활성화
    private void ActivateJoystick(Vector2 touchPosition, bool isActive = true)
    {
        if (isActive)
        {
            // 터치된 위치를 캔버스 좌표로 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,  // 캔버스의 RectTransform
                touchPosition,                    // 터치된 스크린 좌표
                canvas.worldCamera,// 월드 카메라 (Overlay 모드에서는 null 가능)
                out localPoint);                  // 변환된 로컬 좌표 반환

            // 조이스틱을 터치된 위치에 배치 (anchoredPosition 사용)
            // 조이스틱 앵커값에 따른 위치 보정 별도로 실시
            joystick.GetComponent<RectTransform>().anchoredPosition = localPoint + new Vector2(1200,450);
            joystick.SetActive(true);
        }
        else
        {
            joystick.SetActive(false); // 조이스틱 비활성화
        }
    }
}

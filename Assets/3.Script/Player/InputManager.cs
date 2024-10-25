using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public RectTransform leftArea; // UI 영역인 LeftArea
    public RectTransform joystickArea; // Joystick Area
    public LayerMask touchableLayer; // 터치 가능한 레이어

    private Vector2 touchStartPos; // 처음 터치한 위치
    private Vector2 touchCurPos;   // 현재 터치 위치
    public bool isTouching = false; // 터치 중인지 여부

    public Vector2 MoveInput;

    private void Update()
    {
        // 터치스크린이 존재하는지 확인
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // 터치가 시작되었을 때
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                HandleTouchBegan(touchPosition);
            }

            // 터치 중일 때 (손가락이 움직이는 경우)
            if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isTouching)
            {
                HandleTouchMoved(touchPosition);
            }
        }

        // 터치가 끝났을 때
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            MoveInput = new Vector2(0, 0);
            isTouching = false; // 터치가 끝나면 false로 설정
        }
    }

    // 터치가 시작될 때 호출되는 메서드
    private void HandleTouchBegan(Vector2 touchPosition)
    {
        // Joystick Area를 터치했는지 확인 (바로 터치 시작 위치 저장)
        if (RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition))
        {
            // Joystick Area 안에서는 바로 터치 시작 위치를 저장
            touchStartPos = touchPosition;
            isTouching = true; // 터치 중 상태로 변경
        }
        // LeftArea를 터치했는지 확인
        else if (RectTransformUtility.RectangleContainsScreenPoint(leftArea, touchPosition))
        {
            // 터치한 위치에 Touchable Object가 있는지 확인
            if (!IsTouchableObjectAtPosition(touchPosition))
            {
                // 터치 시작 위치를 저장
                touchStartPos = touchPosition;
                isTouching = true; // 터치 중 상태로 변경
            }
        }
    }

    // 터치한 곳에 "Touchable Object"가 있는지 확인하는 메서드
    private bool IsTouchableObjectAtPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableLayer))
        {
            // "Touchable Object" 태그를 가진 오브젝트가 있는지 확인
            if (hit.collider.CompareTag("touchableobject"))
            {
                return true;
            }
        }
        return false;
    }

    // 터치가 이동했을 때 호출되는 메서드
    private void HandleTouchMoved(Vector2 touchPosition)
    {
        // 현재 터치 위치를 저장
        touchCurPos = touchPosition;

        // 터치 시작 위치와 현재 위치의 차이 반환
        Vector2 touchDelta = GetTouchDelta();
        MoveInput = touchDelta.normalized;
    }

    // touchCurPos - touchStartPos 값을 반환하는 메서드
    public Vector2 GetTouchDelta()
    {
        return touchCurPos - touchStartPos;
    }
}

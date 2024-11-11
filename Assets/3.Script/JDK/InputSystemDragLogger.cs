using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemDragLogger : MonoBehaviour
{
    private Vector2 startMousePosition;
    private bool isDragging = false;

    // Input Actions을 참조하기 위한 변수
    private Vector2 currentMousePosition;

    public void OnPoint(InputAction.CallbackContext context)
    {
        // 마우스 위치 업데이트
        currentMousePosition = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started) // 마우스 클릭 시작
        {
            startMousePosition = currentMousePosition;
            isDragging = true;
        }
        else if (context.canceled && isDragging) // 마우스 클릭 종료
        {
            Vector2 dragVector = currentMousePosition - startMousePosition;

            // 드래그 방향에 따라 처리
            if (Mathf.Abs(dragVector.x) > Mathf.Abs(dragVector.y))
            {
                if (dragVector.x > 0)
                {
                    Debug.Log("오른쪽으로 드래그되었습니다.");
                }
                else
                {
                    Debug.Log("왼쪽으로 드래그되었습니다.");
                }
            }
            else
            {
                if (dragVector.y > 0)
                {
                    Debug.Log("위쪽으로 드래그되었습니다.");
                }
                else
                {
                    Debug.Log("아래쪽으로 드래그되었습니다.");
                }
            }

            isDragging = false;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ColliderDirectionTrigger : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Camera mainCamera;

    // Input Action
    public InputAction clickAction;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        mainCamera = Camera.main;

        // Input Action 활성화
        clickAction.Enable();
        clickAction.performed += OnClick;
    }

    private void OnDestroy()
    {
        // Input Action 비활성화
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        // 마우스 또는 터치 위치 가져오기
        Vector2 screenPosition = Pointer.current.position.ReadValue();

        // 스크린 좌표를 월드 Ray로 변환
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // Raycast로 Collider 충돌 감지
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == boxCollider)
            {
                CheckDirection(hit.point);
            }
        }
    }

    private void CheckDirection(Vector3 hitPoint)
    {
        // BoxCollider의 중심과 크기
        Vector3 center = boxCollider.bounds.center;
        Vector3 extents = boxCollider.bounds.extents;

        // hitPoint와 중심을 비교하여 어느 면에 가까운지 확인
        if (Mathf.Abs(hitPoint.x - (center.x + extents.x)) < 0.1f)
        {
            TriggerRightEvent();
        }
        else if (Mathf.Abs(hitPoint.x - (center.x - extents.x)) < 0.1f)
        {
            TriggerLeftEvent();
        }
        else if (Mathf.Abs(hitPoint.y - (center.y + extents.y)) < 0.1f)
        {
            TriggerTopEvent();
        }
        else if (Mathf.Abs(hitPoint.y - (center.y - extents.y)) < 0.1f)
        {
            TriggerBottomEvent();
        }
    }

    private void TriggerRightEvent()
    {
        Debug.Log("Right side hit!");
        // 오른쪽 이벤트 실행 코드
    }

    private void TriggerLeftEvent()
    {
        Debug.Log("Left side hit!");
        // 왼쪽 이벤트 실행 코드
    }

    private void TriggerTopEvent()
    {
        Debug.Log("Top side hit!");
        // 위쪽 이벤트 실행 코드
    }

    private void TriggerBottomEvent()
    {
        Debug.Log("Bottom side hit!");
        // 아래쪽 이벤트 실행 코드
    }
}
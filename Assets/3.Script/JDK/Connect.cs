using UnityEngine;
using UnityEngine.InputSystem; // Input System 네임스페이스 추가

public class Connect : MonoBehaviour
{
    private Camera mainCamera;
    private LineRenderer lineRenderer; // LineRenderer 추가
    private bool isDragging = false; // 드래그 중 여부
    private Vector3 startTouchPosition; // 터치 시작 위치

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>(); // LineRenderer 컴포넌트 가져오기
        lineRenderer.positionCount = 0; // 초기엔 선이 없도록 설정
        lineRenderer.enabled = false; // 처음엔 LineRenderer 비활성화
    }

    void Update()
    {
        // 터치 입력 처리
        if (Touchscreen.current != null)
        {
            var primaryTouch = Touchscreen.current.primaryTouch;

            // 터치가 시작될 때
            if (primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 touchPosition = primaryTouch.position.ReadValue();
                CheckForObject(touchPosition);
                startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));
                isDragging = true;
                lineRenderer.positionCount = 1; // 선의 포지션 갯수 1로 설정 (시작점)
                lineRenderer.SetPosition(0, startTouchPosition); // 시작점 설정
                // lineRenderer.enabled = true; // 드래그가 시작되면 LineRenderer 활성화
            }

            // 터치가 이동할 때
            if (isDragging && primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 touchPosition = primaryTouch.position.ReadValue();
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane));
                worldPosition.x = startTouchPosition.x; // X값은 시작 위치로 고정
                lineRenderer.positionCount = 2; // 선의 포지션 갯수 2로 설정 (시작점과 끝점)
                lineRenderer.SetPosition(1, worldPosition); // Y축에 맞게만 끝점 업데이트
            }

            // 터치가 끝날 때
            if (primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                isDragging = false;
                lineRenderer.enabled = false; // 드래그 끝나면 LineRenderer 비활성화
            }
        }

        // 마우스 입력 처리
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                CheckForObject(mousePosition);
                startTouchPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
                isDragging = true;
                lineRenderer.positionCount = 1; // 선의 포지션 갯수 1로 설정 (시작점)
                lineRenderer.SetPosition(0, startTouchPosition); // 시작점 설정
                lineRenderer.enabled = true; // 드래그가 시작되면 LineRenderer 활성화
            }

            // 마우스 드래그 중일 때
            if (isDragging && Mouse.current.leftButton.isPressed)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
                worldPosition.x = startTouchPosition.x; // X값은 시작 위치로 고정
                lineRenderer.positionCount = 2; // 선의 포지션 갯수 2로 설정 (시작점과 끝점)
                lineRenderer.SetPosition(1, worldPosition); // Y축에 맞게만 끝점 업데이트
            }

            // 마우스 버튼이 떼어졌을 때
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
                lineRenderer.enabled = false; // 드래그 끝나면 LineRenderer 비활성화
            }
        }
    }

    void CheckForObject(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Connect"))
            {
                Debug.Log("터치 완료!");
            }
        }
    }
}

 using UnityEngine;
 using UnityEngine.InputSystem;

public class SlideMove : MonoBehaviour
{
    public LayerMask touchableLayer; // 터치 감지할 레이어 설정
    private GameObject selectedObject; // 현재 터치된 오브젝트
    private bool isObjectSelected;
    private Vector3 initialObjectPosition; // 터치 시작 시 오브젝트의 초기 위치
    private Vector2 initialTouchPosition;  // 터치 시작 시 손가락 위치
    public GameObject correctZone;

    private void Update()
    {
        if (isObjectSelected)
        {
            // 오브젝트가 선택된 상태에서 손가락 이동에 따라 오브젝트를 0.5 단위로 이동
            MoveObjectWithTouchX();
            MoveObjectWithTouchZ();
        }
        else
        {
            // 터치가 시작되면 오브젝트 감지 및 이동 가능 여부 확인
            if (DetectTouchStart())
            {
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                if (DetectObjectAtTouch(touchPosition)) // 감지된 오브젝트가 터치 가능한 레이어에 있을 때
                {
                    isObjectSelected = true;
                    initialTouchPosition = touchPosition; // 터치 시작 위치 저장
                    initialObjectPosition = selectedObject.transform.position; // 오브젝트의 초기 위치 저장
                }
            }
        }

        // 터치가 종료되면 선택 해제
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<Outline>().enabled = false;
                selectedObject = null;
                isObjectSelected = false;
                correctZone.GetComponent<CorrectCheck>().CheckAllRays();
            }
        }
    }

    private bool DetectTouchStart()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touchPhase = Touchscreen.current.primaryTouch.phase.ReadValue();
            return touchPhase == UnityEngine.InputSystem.TouchPhase.Began;
        }
        return false;
    }

    private bool DetectObjectAtTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchableLayer))
        {
            selectedObject = hit.collider.gameObject;
            selectedObject.GetComponent<Outline>().enabled = true;
            return true;
        }
        return false;
    }

    private void MoveObjectWithTouchX()
    {
        if (selectedObject == null) return;

        // 현재 터치 위치 가져오기
        Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        // 이동할 X 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetX(currentTouchPosition);

        // X 방향으로 목표 위치 계산
        Vector3 targetPositionX = initialObjectPosition + new Vector3(moveOffset.x, 0, 0);

        // SlideObject 컴포넌트 가져와서 겹침 여부 확인
        SlideObject slideObject = selectedObject.GetComponent<SlideObject>();
        if (slideObject != null && !slideObject.IsOverlappingAtPosition(targetPositionX))
        {
            // 겹치지 않을 때만 X축 이동
            selectedObject.transform.position = targetPositionX;
            initialObjectPosition = targetPositionX; // 이동 후 초기 위치 업데이트
            Debug.Log("X축 이동");
        }
        else
        {
            initialTouchPosition.y = currentTouchPosition.y;
        }
    }

    private void MoveObjectWithTouchZ()
    {
        if (selectedObject == null) return;

        // 현재 터치 위치 가져오기
        Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        // 이동할 Z 오프셋 계산
        Vector3 moveOffset = CalculateMoveOffsetZ(currentTouchPosition);

        // Z 방향으로 목표 위치 계산
        Vector3 targetPositionZ = initialObjectPosition + new Vector3(0, 0, moveOffset.z);

        // SlideObject 컴포넌트 가져와서 겹침 여부 확인
        SlideObject slideObject = selectedObject.GetComponent<SlideObject>();
        if (slideObject != null && !slideObject.IsOverlappingAtPosition(targetPositionZ))
        {
            // 겹치지 않을 때만 Z축 이동
            selectedObject.transform.position = targetPositionZ;
            initialObjectPosition = targetPositionZ; // 이동 후 초기 위치 업데이트
            Debug.Log("Z축 이동");
        }
        else
        {
            initialTouchPosition.x = currentTouchPosition.x;
        }
    }

    private Vector3 CalculateMoveOffsetX(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;

        // X 방향의 이동 설정
        if (Mathf.Abs(touchDelta.y) >= 0.1f)
        {
            moveOffset.x = -Mathf.Sign(touchDelta.y) * 0.25f;
            initialTouchPosition.y = currentTouchPosition.y; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }

    private Vector3 CalculateMoveOffsetZ(Vector2 currentTouchPosition)
    {
        Vector3 moveOffset = Vector3.zero;
        Vector2 touchDelta = currentTouchPosition - initialTouchPosition;

        // Z 방향의 이동 설정
        if (Mathf.Abs(touchDelta.x) >= 0.1f)
        {
            moveOffset.z = Mathf.Sign(touchDelta.x) * 0.25f;
            initialTouchPosition.x = currentTouchPosition.x; // 이동 후 새로운 기준점 설정
        }

        return moveOffset;
    }
}

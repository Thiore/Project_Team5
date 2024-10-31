using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlideMove : MonoBehaviour
{
    public LayerMask touchableLayer; // 터치 감지할 레이어 설정
    private GameObject selectedObject; // 현재 터치된 오브젝트
    private bool isObjectSelected;
    private Vector3 initialObjectPosition; // 터치 시작 시 오브젝트의 초기 위치
    private Vector2 initialTouchPosition;  // 터치 시작 시 손가락 위치
    private Vector3 lastValidPosition; // 마지막으로 유효했던 위치 저장

    public GameObject correctZone;

    private void Update()
    {
        if (isObjectSelected)
        {
            // 오브젝트가 선택된 상태에서 손가락 이동에 따라 오브젝트를 움직임
            MoveObjectWithTouch();
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
                }
            }
        }

        // 터치가 종료되면 선택 해제
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            if (selectedObject != null)
            {
                Debug.Log($"Selected object name: {selectedObject.gameObject.name}");
                selectedObject.GetComponent<Outline>().enabled = false;
                selectedObject = null;
                isObjectSelected = false;
                correctZone.GetComponent<CorrectCheck>().CheckAllRays();
            }
        }
    }

    private bool DetectTouchStart()
    {
        // 터치스크린이 있는지 확인하고, 터치가 시작되었는지 확인
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            // 터치가 시작되었는지 확인
            var touchPhase = Touchscreen.current.primaryTouch.phase.ReadValue();
            if (touchPhase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }

    private bool DetectObjectAtTouch(Vector2 touchPosition)
    {
        // 터치 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // 지정한 레이어의 오브젝트와 Raycast 충돌 여부 확인
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableLayer))
        {
            selectedObject = hit.collider.gameObject;
            selectedObject.GetComponent<Outline>().enabled = true;
            initialObjectPosition = selectedObject.transform.position; // 오브젝트의 초기 위치 저장
            initialTouchPosition = touchPosition; // 터치 시작 위치 저장
            return true; // 터치 가능한 레이어에 있는 오브젝트를 감지한 경우 true 반환
        }
        return false; // 터치 가능한 오브젝트가 없을 경우 false 반환
    }

    private void MoveObjectWithTouch()
    {
        if (selectedObject == null) return;

        // 터치 이동에 따라 오브젝트 이동
        if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            MoveObject(currentTouchPosition);
        }
    }


    private void MoveObject(Vector2 currentTouchPosition)
    {
        // 터치 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(currentTouchPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            // 이동할 목표 위치 계산
            Vector3 targetPosition = new Vector3(
                Mathf.Round(hitInfo.point.x * 2) * 0.5f,
                selectedObject.transform.position.y,
                Mathf.Round(hitInfo.point.z * 2) * 0.5f
            );

            // SlideObject 컴포넌트 가져오기
            SlideObject slideObject = selectedObject.GetComponent<SlideObject>();
            Vector3 currentPosition = selectedObject.transform.position;
            if (slideObject != null && !slideObject.IsOverlappingAtPosition(targetPosition))
            {
                // 겹치지 않을 때만 이동
                selectedObject.transform.position = targetPosition;
            }
            else
            {
                // 겹치는 경우 초기 위치로 복구
                selectedObject.transform.position = currentPosition;
                //selectedObject.GetComponent<Outline>().enabled=false;
                //selectedObject = null;
                //isObjectSelected = false;
                Debug.Log("Cannot move, object is overlapping with another collider.");
            }
        }
    }



}

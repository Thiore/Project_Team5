using UnityEngine;
using System;

public class GridSpin : MonoBehaviour
{
    public float rotationSpeed = 2f; // 회전 속도
    private Quaternion targetRotation; // 목표 회전값
    private ReadInputData readInput;
    private bool isRotating = false; // 회전 중 여부 확인
    public bool couldRotate = true;

    // 회전 완료 시 실행할 이벤트
    public event Action OnRotationComplete;

    private void Start()
    {
        readInput = GetComponent<ReadInputData>();
        if (readInput == null) Debug.Log("readinput null");
        targetRotation = transform.rotation; // 현재 회전을 초기 목표값으로 설정
    }

    private void Update()
    {
        // 회전 중이 아닐 때만 터치 감지
        if (!isRotating && readInput.isTouch && couldRotate)
        {
            Rotate();
            readInput.TouchTap();
        }

        // 목표 회전으로 부드럽게 회전
        if (isRotating)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 목표 회전에 근사하면 정확히 목표 회전으로 설정
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
                isRotating = false;

                // 회전 완료 시 이벤트 호출
                OnRotationComplete?.Invoke();
            }
        }
    }

    // 90도 회전 설정
    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
        isRotating = true; // 회전 시작
    }
}

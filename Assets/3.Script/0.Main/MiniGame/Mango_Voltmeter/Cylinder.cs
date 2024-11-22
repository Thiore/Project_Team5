 using System;
 using UnityEngine;

public class Cylinder : MonoBehaviour
{
    private int minValue = 0;
    private int maxValue = 180;
    public float rotateSpeed;
    public float correctValue;

    public event Action AfterRotate; // 회전 완료 시 실행할 이벤트

    private Quaternion targetRotation; // 목표 회전값
    private bool isRotating = false; // 회전 중 여부 확인

    private Quaternion SetRotateValue(float rotateValue)
    {
        float targetRotationZ;

        // 목표 회전값 설정 (minValue와 maxValue 사이로 제한)
        if (transform.eulerAngles.z + rotateValue > maxValue)
        {
            targetRotationZ = maxValue;
        }
        else if (transform.eulerAngles.z + rotateValue < minValue)
        {
            targetRotationZ = minValue;
        }
        else
        {
            targetRotationZ = transform.eulerAngles.z + rotateValue;
        }

        return Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, targetRotationZ);
    }

    public void Rotate(float rotateValue)
    {
        targetRotation = SetRotateValue(rotateValue); // 목표 회전값 설정
        isRotating = true; // 회전 시작
    }

    private void Update()
    {
        if (isRotating)
        {
            // 목표 회전으로 부드럽게 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            // 목표 회전에 근사하면 회전 종료
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
                AfterRotate?.Invoke(); // 회전 완료 이벤트 호출
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class MoveRotation : MonoBehaviour
{
    public RectTransform leftArea; // UI 영역인 LeftArea
    private Rigidbody rb;
    public float rotationSpeed = 5f; // 회전 속도 증가
    private float rotationAmount;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 터치가 진행 중인지 확인
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {

            if (!RectTransformUtility.RectangleContainsScreenPoint(leftArea, Touchscreen.current.primaryTouch.position.ReadValue()))
            {
                // 터치 이동(delta) 값 가져오기
                Vector2 touchDelta = Touchscreen.current.primaryTouch.delta.ReadValue();

                // deltaX를 사용해 회전 값 계산
                rotationAmount = touchDelta.x * rotationSpeed;
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (rotationAmount != 0)
        {
            // 회전 적용
            RotateObject(rotationAmount);
            rotationAmount = 0; // 회전 적용 후 초기화
        }
    }

    private void RotateObject(float deltaX)
    {
        Quaternion deltaRotation = Quaternion.Euler(0f, deltaX, 0f);

        // Rigidbody의 회전을 적용 (기존 회전 값에 추가)
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}

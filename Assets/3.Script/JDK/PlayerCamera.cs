using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("카메라 감도 조절 프로퍼티")]
    [SerializeField] private float cameraSpeedX = 0; // 수평 카메라 감도
    [SerializeField] private float cameraSpeedY = 0; // 수직 카메라 감도

    [SerializeField] private RectTransform rect = null; // UI 영역을 정의하는 RectTransform

    private Vector2 lastTouchPosition; // 마지막으로 터치한 위치

    private CinemachinePOV pov = null; // Cinemachine POV 컴포넌트 참조
    private CinemachineVirtualCamera virtualCamera = null; // Cinemachine Virtual Camera 컴포넌트 참조

    private bool isDragging = false; // 드래그 중인지 여부를 나타내는 플래그

    private void Awake()
    {
        // 컴포넌트를 초기화
        virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        // 터치가 있는지 확인
        if (Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.primaryTouch; // 현재 터치 정보를 가져옴

            if (touch.press.isPressed) // 터치가 눌렸는지 확인
            {
                if (!isDragging) // 드래그가 시작되지 않았다면
                {
                    lastTouchPosition = touch.position.ReadValue(); // 마지막 터치 위치 기록
                    isDragging = true; // 드래그 시작
                }
                else // 드래그 중이라면
                {
                    Vector2 currentTouchPosition = touch.position.ReadValue(); // 현재 터치 위치
                    Vector2 delta = currentTouchPosition - lastTouchPosition; // 터치 이동 거리 계산

                    // RectTransform 내부에 있는지 확인
                    bool contains = RectTransformUtility.RectangleContainsScreenPoint(rect, lastTouchPosition);

                    if (contains) // RectTransform 내부에서 터치하고 있다면
                    {
                        // POV 카메라에 delta 값 적용
                        pov.m_HorizontalAxis.Value += delta.x * cameraSpeedX; // 수평 회전
                        pov.m_VerticalAxis.Value -= delta.y * cameraSpeedY; // 수직 회전

                        lastTouchPosition = currentTouchPosition; // 마지막 터치 위치 업데이트
                    }
                    else // RectTransform 외부에서 터치하고 있다면
                    {
                        // 카메라 회전 감도를 0으로 설정하여 회전을 멈춤
                        pov.m_HorizontalAxis.m_MaxSpeed = 0; // 수평 회전 감도 0으로 설정
                        pov.m_VerticalAxis.m_MaxSpeed = 0; // 수직 회전 감도 0으로 설정
                    }
                }
            }
            else // 터치가 끝나면
            {
                isDragging = false; // 드래그 중지
            }
        }
    }

    /* 
    // 카메라 감도를 설정하는 메서드 (현재 사용되지 않음)
    private void SetCameraSpeed(float horizontalSpeed, float verticalSpeed)
    {
        pov.m_HorizontalAxis.m_MaxSpeed = horizontalSpeed; // 수평 감도 설정
        pov.m_VerticalAxis.m_MaxSpeed = verticalSpeed; // 수직 감도 설정
    } 
    */
}
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private InputManager input;

    [Header("카메라 감도 조절 프로퍼티")]
    [SerializeField] private float cameraSpeedX = 0; // 수평 카메라 감도
    [SerializeField] private float cameraSpeedY = 0; // 수직 카메라 감도

    private Vector2 lastTouchPosition; // 마지막으로 터치한 위치

    private CinemachinePOV pov; // Cinemachine POV 컴포넌트 참조
    private CinemachineVirtualCamera virtualCamera; // Cinemachine Virtual Camera 컴포넌트 참조
    private CinemachineInputProvider inputProvider;

    private void Awake()
    {
        input = InputManager.Instance;

        // 컴포넌트를 초기화
        TryGetComponent(out virtualCamera);
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
       
    }
    private void Start()
    {
        lastTouchPosition = Vector2.zero;
        TryGetComponent(out inputProvider);
    }

    private void Update()
    {
        //if (!input.lookData.value.Equals(Vector2.zero))
        //{
        //    Vector2 currentTouchPosition = input.lookData.value;
        //    if (lastTouchPosition.Equals(Vector2.zero))
        //    {
        //        lastTouchPosition = currentTouchPosition; // 마지막 터치 위치 기록
        //        return;
        //    }
        //    Vector2 delta = currentTouchPosition - lastTouchPosition;
        //    pov.m_HorizontalAxis.Value += delta.x* cameraSpeedX; // 수평 회전
        //    pov.m_VerticalAxis.Value -= delta.y * cameraSpeedY; // 수직 회전
        //    lastTouchPosition = currentTouchPosition;
        //}
        //else
        //{
        //    if(!lastTouchPosition.Equals(Vector2.zero))
        //    {
        //        // 카메라 회전 감도를 0으로 설정하여 회전을 멈춤
        //        lastTouchPosition = Vector2.zero;
        //    }
            
        //}
    }
}
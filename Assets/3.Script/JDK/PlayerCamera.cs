using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private InputManager input;

    [Header("카메라 감도 조절 프로퍼티")]
    [Range(1,10)]
    [SerializeField] private float cameraSpeedX; // 수평 카메라 감도
    [Range(1, 10)]
    [SerializeField] private float cameraSpeedY; // 수직 카메라 감도

    private Vector2 lastTouchPosition; // 마지막으로 터치한 위치

    private Vector3 deltaRot;
    //private CinemachinePOV pov; // Cinemachine POV 컴포넌트 참조
    //private CinemachineVirtualCamera virtualCamera; // Cinemachine Virtual Camera 컴포넌트 참조
    //private CinemachineInputProvider inputProvider;

    private void Awake()
    {
        input = InputManager.Instance;

        // 컴포넌트를 초기화
        //TryGetComponent(out virtualCamera);
        //pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
       
    }
    private void Start()
    {
        lastTouchPosition = Vector2.zero;
        deltaRot = Vector3.zero;
        if (cameraSpeedX.Equals(0f))
            cameraSpeedX = 50f;
        if (cameraSpeedY.Equals(0f))
            cameraSpeedY = 50f;
        //TryGetComponent(out inputProvider);
    }

    private void Update()
    {
        if (!input.lookData.value.Equals(Vector2.zero))
        {
            Vector2 currentTouchPosition = input.lookData.value;
            if (lastTouchPosition.Equals(Vector2.zero))
            {
                lastTouchPosition = currentTouchPosition; // 마지막 터치 위치 기록
                return;
            }
            Vector2 delta = currentTouchPosition - lastTouchPosition;
            deltaRot = new Vector3(-delta.y * cameraSpeedY, delta.x * cameraSpeedX);
            lastTouchPosition = currentTouchPosition;
        }
        else
        {
            if (!lastTouchPosition.Equals(Vector2.zero))
            {
                // 카메라 회전 감도를 0으로 설정하여 회전을 멈춤
                lastTouchPosition = Vector2.zero;
            }

        }
    }
    private void FixedUpdate()
    {
        
        transform.Rotate(deltaRot*Time.fixedDeltaTime*10f);
        deltaRot = Vector3.zero;
    }
}
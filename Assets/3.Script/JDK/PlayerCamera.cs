using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private InputManager input;

    [Header("카메라 감도 조절 프로퍼티")]
    [Range(0,10)]
    [SerializeField] private float cameraSpeedX; // 수평 카메라 감도
    [Range(0, 10)]
    [SerializeField] private float cameraSpeedY; // 수직 카메라 감도

    private Vector2 lastTouchPosition; // 마지막으로 터치한 위치

    private Vector3 deltaRot;

    //private void Awake()
    //{
       

       
    //}
    private void Start()
    {
        input = InputManager.Instance;
        lastTouchPosition = Vector2.zero;
        deltaRot = Vector3.zero;
        if (cameraSpeedX.Equals(0f))
            cameraSpeedX = 5f;
        if (cameraSpeedY.Equals(0f))
            cameraSpeedY = 5f;
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
        
        transform.Rotate(deltaRot*Time.fixedDeltaTime*5f);
        deltaRot = Vector3.zero;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
    }
}
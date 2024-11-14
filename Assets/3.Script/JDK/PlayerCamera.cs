using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, ITouchable
{

    [Header("카메라 회전 스피드")]
    [Range(0, 10)]
    private float cameraSpeed;

    //[SerializeField] private Slider slideSpeed;
    private Vector2 lastTouchPosition;

    private Vector3 deltaRot;
    [SerializeField] private Transform player;
    [SerializeField] private float clampRotX;

    private float currentRotationX;

    private void Start()
    {
        TouchManager.Instance.OnLookStarted += OnTouchStarted;
        TouchManager.Instance.OnLookHold += OnTouchHold;
        TouchManager.Instance.OnLookEnd += OnTouchEnd;
        lastTouchPosition = Vector2.zero;
        deltaRot = Vector3.zero;
        cameraSpeed = 2f;
        currentRotationX = transform.localEulerAngles.x; // 누적 X축 회전값을 추적하는 변수
    }
    //private void LateUpdate()
    //{
    //    transform.position = player.position + transform.up * 0.5f;
    //}
    private void FixedUpdate()
    {
        // 터치 delta 값을 기반으로 X축 회전값을 갱신
        currentRotationX -= deltaRot.y * Time.fixedDeltaTime * 5f;
        currentRotationX = Mathf.Clamp(currentRotationX, -clampRotX, clampRotX); // X축 회전 제한

        // Y축 회전 적용 (deltaRot.x는 Y축 회전으로 사용)
        float rotationY = deltaRot.x * Time.fixedDeltaTime * 5f;

        // transform.localEulerAngles에 제한된 회전값을 설정
        transform.localEulerAngles = new Vector3(currentRotationX, transform.localEulerAngles.y + rotationY, 0f);

        // deltaRot 초기화
        deltaRot = Vector3.zero;
    }

    public void CameraSpeed()
    {
        //cameraSpeed = slideSpeed.value;
    }

    public void OnTouchStarted(Vector2 position)
    {
        lastTouchPosition = position;
    }

    public void OnTouchHold(Vector2 position)
    {
        if (!position.Equals(lastTouchPosition))
        {
            Vector2 delta = position - lastTouchPosition;

            deltaRot = new Vector3(delta.x * cameraSpeed, delta.y * cameraSpeed);

            lastTouchPosition = position;
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        deltaRot = Vector3.zero;
        lastTouchPosition = Vector2.zero;
    }
}
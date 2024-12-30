using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlaneTiltController : MonoBehaviour
{
    [SerializeField] private float tiltMultiplier = 80f; // 기울기 정도를 조절하는 배율
    //[SerializeField] private float smoothSpeed = 0.5f; // 회전의 부드러움을 조절하는 속도
    [SerializeField] private Camera cam;
    [SerializeField] private BallObj ball;

    private Vector3 startTilt; // 초기 기울기 값
    private Vector3 currentTilt;
    private Quaternion targetRotation; // 목표 회전 값
    private InputAction planeAction;

    private void Awake()
    {
        TryGetComponent(out PlayerInput input);
        planeAction = input.actions.FindAction("Acceleration");
        planeAction.Enable();
        InputSystem.EnableDevice(Accelerometer.current);
        planeAction.started += ctx => OnTiltStarted(ctx);
        planeAction.performed += ctx => OnTiltPerformed(ctx);
    }
    private void OnEnable()
    {
        startTilt = Vector3.zero;
    }
    private void OnDisable()
    {
        planeAction.Disable();

        planeAction.started -= ctx => OnTiltStarted(ctx);
        planeAction.performed -= ctx => OnTiltPerformed(ctx);
        //ball.SetActive(false);
    }
    private void OnTiltStarted(InputAction.CallbackContext context)
    {
        startTilt = context.ReadValue<Vector3>();
        
        // 현재 가속도계 데이터 가져오기
        currentTilt = context.ReadValue<Vector3>();
        ball.gameObject.SetActive(true);
        cam.gameObject.SetActive(true);
        Reset();


    }

    private void OnTiltPerformed(InputAction.CallbackContext context)
    {
        // 현재 가속도계 데이터 가져오기
        currentTilt = context.ReadValue<Vector3>();
        Vector3 tiltDelta = currentTilt - startTilt;

        // 목표 회전 값 설정
        SetTargetRotation(tiltDelta);

        // 목표 회전 값으로 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*0.6f);
        //cam.Render();
    }

    //private void Update()
    //{
    //    if(startTilt==new Vector3(0, 0, 0))
    //    {
    //        startTilt= Accelerometer.current.acceleration.ReadValue();
    //        return;
    //    }

    //    if (Accelerometer.current != null)
    //    {
    //         현재 가속도계 데이터 가져오기
    //        Vector3 currentTilt = Accelerometer.current.acceleration.ReadValue();

    //         초기 기울기와의 차이값 계산
    //        Vector2 tiltDelta = currentTilt - startTilt;

    //        x.text = "tilt X value : "+tiltDelta.x.ToString();
    //        y.text = "tile Z value : "+tiltDelta.y.ToString();

    //         목표 회전 값 설정
    //        SetTargetRotation(tiltDelta);

    //         목표 회전 값으로 부드럽게 회전
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    //    }
    //}
    public void Reset()
    {
        startTilt = currentTilt;
        transform.rotation = Quaternion.identity;
        ball.transform.position = ball.getStartValue.position;


    }

    private void SetTargetRotation(Vector3 tiltDelta)
    {
        // X와 Z축에 대해 목표 회전 값 계산
        float tiltX = Mathf.Clamp(-tiltDelta.x * tiltMultiplier, -15f, 15f);
        float tiltZ = Mathf.Clamp(-tiltDelta.y * tiltMultiplier, -15f, 15f);

        // 목표 회전 값을 설정
        targetRotation = Quaternion.Euler(-tiltZ, 0, tiltX);
    }

    
}

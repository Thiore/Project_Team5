using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlaneTiltController : MonoBehaviour
{
    [SerializeField] private float tiltMultiplier = 90f; // 기울기 정도를 조절하는 배율
    //[SerializeField] private float smoothSpeed = 0.5f; // 회전의 부드러움을 조절하는 속도
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject ball;

    private Vector3 startTilt; // 초기 기울기 값
    private Quaternion targetRotation; // 목표 회전 값
    private InputAction planeAction;

    private void Awake()
    {
        TryGetComponent(out PlayerInput input);
        planeAction = input.actions.FindAction("Tilt");
    }
    private void OnEnable()
    {
        startTilt = Vector3.zero;
        Debug.Log("enable" + startTilt);
        planeAction.Enable();
        
        planeAction.started += ctx => OnTiltStarted(ctx);
        planeAction.performed += ctx => OnTiltPerformed(ctx);
        

    }
    private void OnDisable()
    {
        planeAction.Disable();

        planeAction.started -= ctx => OnTiltStarted(ctx);
        planeAction.performed -= ctx => OnTiltPerformed(ctx);
        //ball.SetActive(false);
    }

    //private void Start()
    //{
    //    if (Accelerometer.current != null)
    //    {
    //        // 시작 시점의 기울기 값을 저장하여 기준점으로 사용
    //        startTilt = Accelerometer.current.acceleration.ReadValue();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Accelerometer not available on this device. Please run on an actual device.");
    //    }
    //}

    private void OnTiltStarted(InputAction.CallbackContext context)
    {
        InputSystem.EnableDevice(Accelerometer.current);
        Debug.Log("start"+startTilt);
        startTilt = context.ReadValue<Vector3>();
        Debug.Log("start" + startTilt);
        // 현재 가속도계 데이터 가져오기
        Vector3 currentTilt = context.ReadValue<Vector3>();
        Vector3 tiltDelta = currentTilt - startTilt;

        // 목표 회전 값 설정
        SetTargetRotation(tiltDelta);

        // 목표 회전 값으로 부드럽게 회전
        transform.rotation = targetRotation;
        ball.SetActive(true);
    }

    private void OnTiltPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("performed" + startTilt);
        // 현재 가속도계 데이터 가져오기
        Vector3 currentTilt = context.ReadValue<Vector3>();
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

    private void SetTargetRotation(Vector3 tiltDelta)
    {
        // X와 Z축에 대해 목표 회전 값 계산
        float tiltX = Mathf.Clamp(-tiltDelta.x * tiltMultiplier, -15f, 15f);
        float tiltZ = Mathf.Clamp(-tiltDelta.y * tiltMultiplier, -15f, 15f);

        // 목표 회전 값을 설정
        targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);
    }

    
}

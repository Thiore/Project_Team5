using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlaneTiltController : MonoBehaviour
{
    private Vector3 startTilt=new Vector3(0,0,0); // 초기 기울기 값
    public float tiltMultiplier = 90f; // 기울기 정도를 조절하는 배율
    public float smoothSpeed = 2f; // 회전의 부드러움을 조절하는 속도

    [SerializeField] private Text x;
    [SerializeField] private Text y;
    [SerializeField] private Text z;

    private Quaternion targetRotation; // 목표 회전 값

    private void Start()
    {
        if (Accelerometer.current != null)
        {
            // 시작 시점의 기울기 값을 저장하여 기준점으로 사용
            startTilt = Accelerometer.current.acceleration.ReadValue();
        }
        else
        {
            Debug.LogWarning("Accelerometer not available on this device. Please run on an actual device.");
        }
    }

    private void Update()
    {
        //if(startTilt==new Vector3(0, 0, 0))
        //{
        //    startTilt= Accelerometer.current.acceleration.ReadValue();
        //    return;
        //}

        if (Accelerometer.current != null)
        {
            // 현재 가속도계 데이터 가져오기
            Vector3 currentTilt = Accelerometer.current.acceleration.ReadValue();

            // 초기 기울기와의 차이값 계산
            Vector3 tiltDelta = currentTilt - startTilt;

            x.text = "tilt X value : "+tiltDelta.x.ToString();
            y.text = "tile Z value : "+tiltDelta.y.ToString();

            // 목표 회전 값 설정
            SetTargetRotation(tiltDelta);

            // 목표 회전 값으로 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }

    private void SetTargetRotation(Vector3 tiltDelta)
    {
        // X와 Z축에 대해 목표 회전 값 계산
        float tiltX = Mathf.Clamp(-tiltDelta.x * tiltMultiplier, -5f, 5f);
        float tiltZ = Mathf.Clamp(-tiltDelta.y * tiltMultiplier, -5f, 5f);

        // 목표 회전 값을 설정
        targetRotation = Quaternion.Euler(tiltX, 0, tiltZ);
    }
}

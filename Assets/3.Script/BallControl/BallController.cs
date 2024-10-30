using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;

    private Vector3 tiltInput;

    [SerializeField] private Text x;
    [SerializeField] private Text y;
    [SerializeField] private Text z;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 가속도계 지원 여부 확인
        if (Accelerometer.current == null)
        {
            Debug.LogError("Accelerometer is not available on this device.");
        }
    }

    private void Update()
    {
        // 입력 값 받아오기
        tiltInput = ReadTiltInput();

        // 디버그를 위한 UI 업데이트
        x.text = $"X : {tiltInput.x.ToString("F2")}";
        y.text = $"Y : {tiltInput.y.ToString("F2")}";
        z.text = $"Z : {tiltInput.z.ToString("F2")}";
    }

    private void FixedUpdate()
    {
        // 기울기에 따른 힘을 Rigidbody에 적용하여 공 이동
        Vector3 force = new Vector3(tiltInput.z, 0, -tiltInput.x) * speed;
        rb.AddForce(force);
    }

    private Vector3 ReadTiltInput()
    {
        // Accelerometer 데이터 읽기
        if (Accelerometer.current != null)
        {
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            return new Vector3(acceleration.x, 0, acceleration.y);
        }

        return Vector3.zero; // 가속도계가 없는 경우 (또는 에뮬레이터에서 실행 중일 때)
    }
}

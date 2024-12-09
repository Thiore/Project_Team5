using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

public class RedConnection : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask layer_mask; // Raycast에 사용할 레이어 마스크

    public UnityEvent<Vector2> onTouchStarted; // 터치 시작 이벤트
    public UnityEvent<Vector2> onTouchHold;    // 터치 유지 이벤트
    public UnityEvent<Vector2> onTouchEnd;     // 터치 종료 이벤트

    private Ray ray;               // 화면 좌표를 월드 좌표로 변환하는 Ray
    private RaycastHit hit;        // Ray가 맞은 대상 정보
    private LineRenderer line_renderer = null; // LineRenderer 컴포넌트

    private bool drag_state = false; // 드래그 중인지 여부를 확인하는 플래그

    private void Awake()
    {
        // LineRenderer 컴포넌트를 가져옴
        TryGetComponent(out line_renderer);
    }

    public void OnTouchStarted(Vector2 position)
    {
        onTouchStarted?.Invoke(position); // 터치 시작 시 이벤트 실행

        // 현재 오브젝트의 태그가 "RedConnection"인 경우에만 실행
        if (CompareTag("RedConnection"))
        {
            // Ray가 충돌한 오브젝트가 있고, 태그가 "Black"인 경우 해당 오브젝트 활성화
            if (hit.collider != null && hit.collider.CompareTag("Black"))
            {
                hit.collider.gameObject.SetActive(true);
            }

            // LineRenderer 활성화
            line_renderer.enabled = true;

            // 자식 오브젝트(예: 시각적 효과) 활성화
            transform.GetChild(0).gameObject.SetActive(true);

            // LineRenderer의 시작 위치를 현재 오브젝트의 위치로 설정
            line_renderer.SetPosition(0, transform.position);
            line_renderer.SetPosition(1, transform.position);

            drag_state = true; // 드래그 상태 활성화
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        onTouchHold?.Invoke(position); // 터치 유지 시 이벤트 실행

        if (drag_state)
        {
            // 화면 좌표를 월드 좌표로 변환하여 Ray 생성
            ray = Camera.main.ScreenPointToRay(position);

            // Ray가 레이어 마스크에 맞는 오브젝트와 충돌하면
            if (Physics.Raycast(ray, out hit, 40, layer_mask))
            {
                // LineRenderer의 끝 점 위치를 Ray 충돌 지점으로 설정
                line_renderer.SetPosition(1, hit.point);
            }

            // Ray가 "BlackConnection" 태그를 가진 오브젝트와 충돌하면
            if (hit.collider.CompareTag("BlackConnection"))
            {
                // 자식 오브젝트 비활성화
                transform.GetChild(0).gameObject.SetActive(false);

                // LineRenderer 비활성화 및 드래그 상태 종료
                line_renderer.enabled = false;
                drag_state = false;
            }
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        onTouchEnd?.Invoke(position); // 터치 종료 시 이벤트 실행

        if (drag_state)
        {
            // Ray가 "Black" 태그를 가진 오브젝트와 충돌했으면
            if (hit.collider.CompareTag("Black"))
            {
                Debug.Log("Red 연결 완료!"); // 연결 완료 로그 출력

                // 충돌한 오브젝트 비활성화
                hit.collider.gameObject.SetActive(false);
            }
            else
            {
                // LineRenderer 비활성화 및 드래그 상태 종료
                line_renderer.enabled = false;
                drag_state = false;
            }
        }
    }
}
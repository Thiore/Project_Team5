using UnityEngine;
using UnityEngine.Events;

public class RedConnection : MonoBehaviour, ITouchable
{
    [SerializeField] private LayerMask layer_mask;

    public UnityEvent<Vector2> onTouchStarted;
    public UnityEvent<Vector2> onTouchHold;
    public UnityEvent<Vector2> onTouchEnd;

    private Ray ray;
    private RaycastHit hit;
    private LineRenderer line_renderer = null;

    private int success_value = 0;
    private bool drag_state = false;
    private bool line_state = false;

    private void Awake()
    {
        TryGetComponent(out line_renderer);
    }

    public void OnTouchStarted(Vector2 position)
    {
        onTouchStarted?.Invoke(position);

        if (CompareTag("RedConnection"))
        {
            // 첫 번째 라인 설정
            line_renderer.enabled = true;
            line_renderer.SetPosition(0, transform.position);
            line_renderer.SetPosition(1, transform.position);

            line_state = true;
            drag_state = true;
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        onTouchHold?.Invoke(position);

        if (drag_state)
        {
            ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit, 20, layer_mask))
            {
                line_renderer.SetPosition(1, hit.point);
            }
        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        onTouchEnd?.Invoke(position);

        if (drag_state)
        {
            if (hit.collider.CompareTag("Red") && line_state)
            {
                Debug.Log("Red 태그에 닿았습니다!");

                // 새 LineRenderer 생성
                CreateNewLine(hit.point);

                line_state = false;
            }
            else
            {
                // 닿지 않았을 경우 기존 LineRenderer 비활성화
                line_renderer.enabled = false;
                drag_state = false;
            }
        }
    }

    private void CreateNewLine(Vector3 startPoint)
    {
        // 새로운 LineRenderer 생성
        GameObject newLineObject = new GameObject("NewLine");
        LineRenderer newLineRenderer = newLineObject.AddComponent<LineRenderer>();

        // 새 LineRenderer의 설정 (기본 설정)
        newLineRenderer.positionCount = 2;
        newLineRenderer.SetPosition(0, startPoint);
        newLineRenderer.SetPosition(1, startPoint);
        newLineRenderer.enabled = true;

        // LineRenderer 스타일 설정 (예: 색상, 두께 등)
        newLineRenderer.startWidth = 0.1f;
        newLineRenderer.endWidth = 0.1f;
        newLineRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = Color.red };
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }
}

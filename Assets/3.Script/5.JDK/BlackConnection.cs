using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

public class BlackConnection : MonoBehaviour, ITouchable
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
        
        if (CompareTag("BlackConnection"))
        {
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
            if (hit.collider.tag == "Black" && line_state)
            {
                Debug.Log($"Black 태그에 닿았습니다!");

                line_state = false;
            }
            else
            {
                line_renderer.enabled = false;
                drag_state = false;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

public class ReadTouchData : MonoBehaviour,ITouchable
{
    public UnityEvent<Vector2> onTouchStarted;
    public UnityEvent<Vector2> onTouchHold;
    public UnityEvent<Vector2> onTouchEnd;
    public void OnTouchStarted(Vector2 position)
    {
        onTouchStarted?.Invoke(position);
    }

    public void OnTouchHold(Vector2 position)
    {
        onTouchHold?.Invoke(position);
    }

    public void OnTouchEnd(Vector2 position)
    {
        onTouchEnd?.Invoke(position);
    }
}
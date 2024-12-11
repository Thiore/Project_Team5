using UnityEngine;
using UnityEngine.Events;

public class ReadTouchData : MonoBehaviour,ITouchable
{
    [SerializeField] private GameObject cam;
    [SerializeField] private Collider col;
    [SerializeField] private Outline outline;
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
    public void StoryObject()
    {
        if(cam != null)
        {
            if(!cam.activeInHierarchy)
            {
                cam.SetActive(true);
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.SetBtn(false);
                }
                if (TouchManager.Instance != null)
                {
                    TouchManager.Instance.EnableMoveHandler(false);
                }
            }
            else
            {
                cam.SetActive(false);
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.SetBtn(true);
                }
                if (TouchManager.Instance != null)
                {
                    TouchManager.Instance.EnableMoveHandler(true);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseResultBtn : MonoBehaviour, ITouchable
{
    [SerializeField] private InteractionFuseBox fuseSwithManager;

    private Outline outline;
    private void Awake()
    {
        TryGetComponent(out outline);
        outline.enabled = false;
    }
    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                fuseSwithManager.CheckResult();
            }
        }

    }

    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchStarted(Vector2 position)
    {

    }
}

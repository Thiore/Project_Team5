using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour,ITouchable
{
    [SerializeField] private bool is1F; // 애니메이터의 B1F가 False라면 위로 올라감
    [SerializeField] private Animator liftAnim;

    private readonly int animParams = Animator.StringToHash("B1F");

    private Outline outline;

    private void Awake()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;
    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                liftAnim.SetBool(animParams, is1F);
            }
        }
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchStarted(Vector2 position)
    {
        
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
            outline.enabled = false;
        }
    }
}

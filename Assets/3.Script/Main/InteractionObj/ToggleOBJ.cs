using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : MonoBehaviour, ITouchable
{
    [SerializeField] private GameObject cinemachine;
    private Animator anim;
    private readonly int openAnim = Animator.StringToHash("Open");
    private Outline outline;

    private bool isTouching;
    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;

        if (!TryGetComponent(out anim))
        {
            transform.parent.TryGetComponent(out anim);
        }

        isTouching = false;
    }

    public void OnTouchStarted(Vector2 position)
    {
        Debug.Log(isTouching);
        isTouching = !isTouching;

        cinemachine.SetActive(isTouching);
        anim.SetBool(openAnim, isTouching);

    }
    public void OnTouchHold(Vector2 position)
    {

    }
    public void OnTouchEnd(Vector2 position)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.enabled = false;
        }
    }
}

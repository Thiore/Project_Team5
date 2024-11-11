using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : InteractionOBJ, ITouchable
{
    [SerializeField] private GameObject cinemachine;
    private Animator anim;
    private readonly int openAnim = Animator.StringToHash("Open");
    protected override void OnEnable()
    {
        TryGetComponent(out anim);
    }

    public void OnTouchStarted(Vector2 position)
    {
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
}

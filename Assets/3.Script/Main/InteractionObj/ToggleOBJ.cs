using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : InteractionOBJ
{
    [SerializeField] private GameObject cinemachine;
    private Animator anim;
    private readonly int openAnim = Animator.StringToHash("Open");
    protected override void OnEnable()
    {
        TryGetComponent(out anim);
    }

    public override void OnTouchStarted(Vector2 position)
    {
        base.OnTouchStarted(position);
        isTouching = !isTouching;

        cinemachine.SetActive(isTouching);
        anim.SetBool(openAnim, isTouching);

    }
    public override void OnTouchHold(Vector2 position)
    {
        base.OnTouchHold(position);
    }
    public override void OnTouchEnd(Vector2 position)
    {
        base.OnTouchEnd(position);
    }
}

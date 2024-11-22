 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class InteractionOBJ : MonoBehaviour
{
    private Outline outline;

    [Header("기본 Cinemachine카메라")]
    [SerializeField] protected GameObject normalCamera;


    protected bool isTouching;

    protected readonly int openAnim = Animator.StringToHash("Open");

    protected Animator anim;

    protected virtual void Start()
    {
        if(TryGetComponent(out outline))
            outline.enabled = false;
        Debug.Log("InteractionObj");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")&&outline != null)
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

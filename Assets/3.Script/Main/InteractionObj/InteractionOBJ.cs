using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOBJ : MonoBehaviour
{
    private Outline outline;

    protected bool isTouching;


    
    protected virtual void OnEnable()
    {
        TryGetComponent(out outline);

        if(outline.enabled)
            outline.enabled = false;

        isTouching = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            outline.enabled = false;
        }
    }
}

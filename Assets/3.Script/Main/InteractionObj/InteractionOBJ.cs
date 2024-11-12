using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOBJ : MonoBehaviour
{
    private Outline outline;

    protected bool isTouching = false;


    
    protected virtual void Start()
    {
        if(TryGetComponent(out outline))
            outline.enabled = false;

        Debug.Log("outline");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainPlayer"))
        {
            outline.enabled = false;
        }
    }
}

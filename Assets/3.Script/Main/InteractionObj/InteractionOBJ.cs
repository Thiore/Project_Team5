using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOBJ : MonoBehaviour, ITouchable
{
    private Outline outline;

    protected bool isTouching;


    public virtual void OnTouchStarted(Vector2 position)
    {
        

    }
    public virtual void OnTouchHold(Vector2 position)
    {
        
    }    
    public virtual void OnTouchEnd(Vector2 position)
    {

    }

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

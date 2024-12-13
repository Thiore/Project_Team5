using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{
    private Outline outline;
    [SerializeField] private GameObject cam;
    private Item myItem;
    private bool isTrigger;

    private void Start()
    {
        isTrigger = false;
        if (TryGetComponent(out outline))
            outline.enabled = false;

        //TryGetComponent(out myItem);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
            isTrigger = true;
        
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(isTrigger&&!outline.enabled)
        {
            if (cam != null)
            {
                if (cam.activeInHierarchy)
                {
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    else
                    {
                        outline.enabled = false;
                    }
                }

            }
            else
            {
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (outline != null && other.CompareTag("MainCamera"))
        {
            outline.enabled = false;
        }
    }
}

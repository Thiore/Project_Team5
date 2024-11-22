using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{
    private Outline outline;
    [SerializeField] private GameObject cam;
    private Item myItem;

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;

        //TryGetComponent(out myItem);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(cam != null)
        {
            if(cam.activeSelf)
            {
                if (outline != null && other.CompareTag("MainCamera"))
                {
                    outline.enabled = true;
                }
            }

        }
        else
        {
            if (outline != null && other.CompareTag("MainCamera"))
            {
                outline.enabled = true;
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

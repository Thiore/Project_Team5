using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{
    private Outline outline;

    private void Start()
    {
        if (TryGetComponent(out outline))
            outline.enabled = false;

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

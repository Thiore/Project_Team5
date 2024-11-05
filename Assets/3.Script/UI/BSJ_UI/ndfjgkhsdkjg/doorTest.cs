using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTest : MonoBehaviour
{
    private Animator ani;
    private ReadInputData input;
    private bool isOpend = false;
    private void Start()
    {
        ani = GetComponent<Animator>();
        TryGetComponent(out input);
        
    }

    private void Update()
    {
        if (!isOpend &&input.isTouch)
        {
            ani.SetTrigger("Open");
            isOpend = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBasic : MonoBehaviour
{
    private ReadInputData input;
    private Animator ani;
    private bool isOpen;
    private void Start()
    {
        TryGetComponent(out input);
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (input.isTouch && !isOpen)
        {
            
            ani.SetBool("isOpen", true);
            isOpen = true;
        }
        else if(input.isTouch && isOpen)
        {
            
            ani.SetBool("isOpen", false);
            isOpen = false;
        }
    }
}

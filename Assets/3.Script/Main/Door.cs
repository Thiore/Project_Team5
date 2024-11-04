using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private ReadInputData input;

    private Animator anim;

    private bool isOpen;

    private void Start()
    {
        TryGetComponent(out input);
        TryGetComponent(out anim);
        isOpen = false;
    }
    private void Update()
    {
        if(input.isTouch)
        {
                isOpen = !isOpen;
                if (isOpen)
                {
                    anim.SetTrigger("Open");
                    input.TouchTap();

                }
                else
                {
                    anim.SetTrigger("Close");
                    input.TouchTap();

                }
            
            Debug.Log(isOpen);
        }
    }
}

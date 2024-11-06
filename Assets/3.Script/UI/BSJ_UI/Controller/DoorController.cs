using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator ani;
    private ReadInputData input;
    private bool isOpend = false;

    //상호작용 object 참조
    [SerializeField] private GameObject openObj;

    private void Start()
    {
        ani = GetComponent<Animator>();

        TryGetComponent(out input);

    }

    private void Update()
    {
        if (input.isTouch && !isOpend)
        {
            ani.SetTrigger("Open");
            isOpend = true;
        }
    }
}

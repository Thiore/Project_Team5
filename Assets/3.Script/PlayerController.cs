using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb = null;

    [SerializeField] private float speed = 0;

    private float move_x = 0;
    private float move_y = 0;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.D))
        {

        }

        if (Input.GetKeyDown(KeyCode.W))
        {

        }

        if (Input.GetKeyDown(KeyCode.S))
        {

        }
    }
}
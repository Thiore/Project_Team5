using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private InputManager input;
    private Rigidbody rb;

    public float speed;

    Vector3 moveDir;

    private void Awake()
    {
        input = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDir = new Vector3(input.MoveInput.x, 0, input.MoveInput.y).normalized;
    }
    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.Move(rb.position+moveDir*speed*Time.fixedDeltaTime,Quaternion.identity);
        }        
    }
}

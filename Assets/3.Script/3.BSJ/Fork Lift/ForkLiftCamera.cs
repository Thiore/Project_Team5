using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForkLiftCamera : MonoBehaviour
{
    private Vector2 lookInput;
    [Range(1, 10)]
    [SerializeField] private float cameraSpeed;

    private void Start()
    {
        lookInput = Vector2.zero;
    }
    private void Update()
    {
        Vector3 lookDir = new Vector3(-lookInput.y, lookInput.x, 0f);
        transform.Rotate(lookDir * cameraSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        Debug.Log(lookInput);
    }
}

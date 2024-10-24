using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    private void Start()
    {
        Debug.Log("카메라 로직 실행");
    }

    private void Update()
    {
        float pos_x = Input.GetAxisRaw("Horizontal");
        float pos_y = Input.GetAxisRaw("Vertical");

        this.transform.Rotate(Vector3.up, pos_x * speed * Time.deltaTime);
        this.transform.Rotate(Vector3.left, pos_y * speed * Time.deltaTime);
    }
}
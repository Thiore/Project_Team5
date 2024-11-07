using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour, ITouchable
{
    private InputManager input;

    [Header("카메라 회전 스피드")]
    [Range(0, 10)]
    private float cameraSpeed;

    //[SerializeField] private Slider slideSpeed;
    private Vector2 lastTouchPosition;

    private Vector3 deltaRot;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        input = InputManager.Instance;
        lastTouchPosition = Vector2.zero;
        deltaRot = Vector3.zero;
        cameraSpeed = 2f;
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        transform.Rotate(deltaRot*Time.fixedDeltaTime*5f);
        deltaRot = Vector3.zero;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
    }
    public void CameraSpeed()
    {
        //cameraSpeed = slideSpeed.value;
    }

    public void OnTouchStarted(Vector2 position)
    {
        lastTouchPosition = position;
    }

    public void OnTouchHold(Vector2 position)
    {
        if (!position.Equals(lastTouchPosition))
        {
            Vector2 delta = position - lastTouchPosition;
            deltaRot = new Vector3(-delta.y * cameraSpeed, delta.x * cameraSpeed);
            //lastTouchPosition = currentTouchPosition;
        }
        else
        {
            if (!lastTouchPosition.Equals(Vector2.zero))
            {
                lastTouchPosition = Vector2.zero;
            }

        }
    }

    public void OnTouchEnd(Vector2 position)
    {
        throw new System.NotImplementedException();
    }
}
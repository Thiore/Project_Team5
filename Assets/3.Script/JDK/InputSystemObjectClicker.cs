using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputSystemObjectClicker : MonoBehaviour
{
    public Camera mainCamera; // 메인 카메라 참조

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // Pointer 입력 처리 (마우스 클릭 또는 터치)
    public void OnPointerDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Clicked on: {hit.collider.gameObject.name}");
            }
        }
    }
}

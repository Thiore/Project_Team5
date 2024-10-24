using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse = null;
    [SerializeField] private float speed = 5f; // 속도를 초기화

    private void OnEnable()
    {
        moveActionToUse.action.Enable(); // 액션 활성화
    }

    private void OnDisable()
    {
        moveActionToUse.action.Disable(); // 액션 비활성화
    }

    private void Update()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();

        // 이동 처리
        this.transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum eBallType
{
    Normal = 0,
    Blue,
    Red,
    Green
}
public class BallController : MonoBehaviour
{
    [SerializeField] private Transform plane;
    [SerializeField] private Transform startValue;
    [SerializeField] private Transform endValue;

    public eBallType ballType;

    private bool isClear;

    private Rigidbody rigid;

    private void OnEnable()
    {
        TryGetComponent(out rigid);
        ballType = eBallType.Normal;
        isClear = false;
        transform.position = startValue.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GameController"))
        {
            isClear = true;
        }
    }

}

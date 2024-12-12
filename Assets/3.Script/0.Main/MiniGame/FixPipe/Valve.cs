using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Valve : MonoBehaviour, ITouchable
{
    [SerializeField] private FixPipeGameManager pipegameManager;
    //0 90(left) -180(up) -90
    [SerializeField] private bool ispipecheck; // 파이프 연결 확인해야하는경우 true

    private Vector3 rotationAxis = Vector3.forward; // 회전 축
    private float duration = 0.6f; // N초 동안 회전
    private float angle = 90f; // 90도 회전
    private bool isRotating;

    private Valve nextValve;
    private Pipe directionPipe;
    public Pipe DirectionPipe { get => directionPipe; }




    //angler z 받아서 enum 으로 방향판단?
    // 90 180 -90 0 
    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject) && !isRotating)
            {
                RotateValve();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Pipe pipe))
        {
            pipe.SetIsImageready();
            directionPipe = pipe;
 
        }

        if (other.gameObject.TryGetComponent(out Valve valve))
        {
            nextValve = valve;
            Debug.Log(nextValve.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Pipe pipe))
        {
            pipe.SetIsImageready();
            directionPipe = null;
        }
        if (other.gameObject.TryGetComponent(out Valve valve))
        {
            nextValve = null;
        }
    }

    public void RotateValve()
    {
        StartCoroutine(RotateValve_co());
    }

    private IEnumerator RotateValve_co()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(rotationAxis * angle);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.fixedDeltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        transform.rotation = endRotation; // 정확한 최종 값 설정
        isRotating = false;

        pipegameManager.FindPath();
    }


    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchStarted(Vector2 position)
    {

    }

    public Valve FindNextValve()
    {
        return nextValve;
    }

}

using System;
using System.Collections;
using UnityEngine;

public class Battery : MonoBehaviour, ITouchable
{
    [SerializeField] private bool isConnection;
    public bool isRed;
    public bool isBlack;

    public bool isStart;


    public event Action CheckBattery;

    
    public void OnTouchStarted(Vector2 position)
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
        if (isStart&&!isConnection&&!isRed && !isBlack)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    transform.Rotate(Vector3.up, 90f, Space.Self);
                }
            }
        }
    }
    public void ConnectingColor(eConnection connectingColor, bool isConnect = false)
    {
        switch (connectingColor)
        {
            case eConnection.Red:
                isRed = isConnect;
                break;
            case eConnection.Black:
                isBlack = isConnect;
                break;
        }
        if(isBlack&&isRed)
        {
            CheckBattery?.Invoke();
        }
    }

    
}
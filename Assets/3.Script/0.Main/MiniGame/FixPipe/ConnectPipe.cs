using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPipe : Pipe, ITouchable
{
    private bool isconnect = false;
    public bool IsConnect { get => isconnect; }
    [SerializeField] private Image monitorImage;
    [SerializeField] private Image shortpipeimage;

    private void Awake()
    {
        TryGetComponent(out render);
        render.enabled = false;
        monitorImage.enabled = false;
    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                TogglePipeConnection();
            }
        }
    }


    private void TogglePipeConnection()
    {
        render.enabled = !render.enabled;
        isconnect = !isconnect;
        monitorImage.enabled = !monitorImage.enabled;

    }

    public override void PipeImageSet()
    {

        if (isconnect)
        {
            base.PipeImageSet();

            if (shortpipeimage.gameObject.activeSelf)
            {
                shortpipeimage.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!shortpipeimage.gameObject.activeSelf)
            {
                shortpipeimage.gameObject.SetActive(true);
            }
        }
    }

    public void ShotPipeImageSet()
    {
        if (!shortpipeimage.gameObject.activeSelf)
        {
            shortpipeimage.gameObject.SetActive(true);
        }
    }

    public void OnTouchHold(Vector2 position)
    {

    }

    public void OnTouchStarted(Vector2 position)
    {

    }
}

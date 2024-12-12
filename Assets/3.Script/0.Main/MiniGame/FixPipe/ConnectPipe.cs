using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPipe : Pipe, ITouchable
{
    private MeshRenderer render;
    private bool isconnect = false;
    [SerializeField] private Image shortpipeimage;

    private void Awake()
    {
        TryGetComponent(out render);
        render.enabled = false;
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

    }

    public void ToggelPipeImage()
    {
        if (isconnect)
        {
            pipeimage.enabled = true;
            shortpipeimage.enabled = false;
        }
        else
        {
            pipeimage.enabled = false;
            shortpipeimage.enabled = true;
        }
    }

    public override void PipeImageSet()
    {
       
        if (isconnect)
        {
            base.PipeImageSet();
        }
        else
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

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
    private Color fillColor;
    private Outline outline;

    private void Awake()
    {
        TryGetComponent(out render);
        fillColor = new Color(render.material.color.r, render.material.color.g, render.material.color.b,1f);
        render.material.color = Color.clear;
        monitorImage.enabled = false;
        TryGetComponent(out outline);
        outline.enabled = false;
    }

    public void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                pipegameManager.ConnectPipeSet(this);
            }
        }
    }


    public void TogglePipeConnection()
    {
        isconnect = !isconnect;
        monitorImage.enabled = !monitorImage.enabled;
        if(isconnect)
        {
            render.material.color = fillColor;
            outline.enabled = false;
        }
        else
        {
            render.material.color = Color.clear;
            outline.enabled = true;
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null&& !isconnect)
        {
            outline.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }
}

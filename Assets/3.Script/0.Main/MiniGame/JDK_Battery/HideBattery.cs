using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBattery : MonoBehaviour
{
    private Material material;
    private Outline outline;

    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;


    [SerializeField] private InteractionBattery batteryManager;

    private readonly Color fillColor = Color.white;
    private readonly Color halfColor = new Color(1f, 1f, 1f, 0.5f);
    private readonly Color clearColor = Color.clear;

    private void Awake()
    {
        TryGetComponent(out MeshRenderer renderer);
        material = renderer.material;
        TryGetComponent(out outline);
        outline.enabled = false;
    }
    private void Start()
    {
        if(DataSaveManager.Instance.GetGameState(floorIndex,objectIndex))
        {
            material.color = fillColor;
        }
        else
        {
            material.color = clearColor;
        }
    }

    public void IsInteracted(bool touchEnd = false)
    {
        if(!touchEnd)
        {
            if (!outline.enabled)
                outline.enabled = true;
            material.color = halfColor;
        }
        else
        {
            if (outline.enabled)
                outline.enabled = false;
            material.color = fillColor;
            batteryManager.InteractionObject(objectIndex);
        }
    }
    public void HideMaterial()
    {
        outline.enabled = false;
        material.color = clearColor;
    }
}

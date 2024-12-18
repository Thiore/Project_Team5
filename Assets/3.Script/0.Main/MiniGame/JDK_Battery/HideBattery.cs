using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBattery : MonoBehaviour
{
    private Material material;

    private int floorIndex;
    private int objectIndex;

    private bool isInteracted;

    private readonly Color fillColor = Color.white;
    private readonly Color halfColor = new Color(1f, 1f, 1f, 0.5f);
    private readonly Color clearColor = Color.clear;

    private void Awake()
    {
        TryGetComponent(out MeshRenderer renderer);
        material = renderer.material;
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
            material.color = halfColor;
        }
        else
        {
            material.color = fillColor;
        }
    }
    public void HideMaterial()
    {
        material.color = clearColor;
    }
}

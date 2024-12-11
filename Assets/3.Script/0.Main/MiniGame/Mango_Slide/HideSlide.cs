using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSlide : SlideObject
{
    public int floorIndex;
    [SerializeField] private int objIndex;
    public int getObjIndex{get=>objIndex;}
    
    private MeshRenderer meshRenderer;

    private float cubeColor;
    private Color fillColor;
    private Color hideColor = Color.clear;
    private Color halfColor;

    public bool isClear { get; private set; }


    

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out meshRenderer);
        cubeColor = meshRenderer.material.color.r;
        fillColor = new Color(cubeColor, cubeColor, cubeColor, 1f);
        halfColor = new Color(cubeColor, cubeColor, cubeColor, 0.6f);
        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objIndex);
        if(isClear)
        {
            meshRenderer.material.color = fillColor;
        }
        else
        {
            meshRenderer.material.color = hideColor;
        }
    }

    public bool IsInteracted(int objId, bool touchEnd)
    {
        if(!isClear)
        {
            if (objId.Equals(objIndex))
            {
                if (touchEnd)
                {
                    meshRenderer.material.color = fillColor;
                    return true;
                }
                else
                {
                    meshRenderer.material.color = halfColor;
                    return false;
                }
                
            }
            meshRenderer.material.color = hideColor;
        }
        return false;
    }
    public void HideMaterial()
    {
        meshRenderer.material.color = hideColor;
    }
    public override void OnTouchEnd(Vector2 position)
    {
        base.OnTouchEnd(position);
        if(objIndex.Equals(5))
        {
            puzzleObj.CheckAllRays(this.gameObject);
            
        }
    }
}

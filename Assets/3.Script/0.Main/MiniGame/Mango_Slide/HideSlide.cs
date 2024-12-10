using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSlide : SlideObject
{
    public int floorIndex;
    [SerializeField] private int objIndex;
    public int getObjIndex{get=>objIndex;}
    
    private MeshRenderer meshRenderer;
    
    private Color fillColor = Color.white;
    private Color hideColor = Color.clear;
    private Color halfColor = new Color(1f, 1f, 1f, 0.6f);

    public bool isClear { get; private set; }


    

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out meshRenderer);
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
                    isClear = true;
                }
                else
                {
                    meshRenderer.material.color = halfColor;
                }
                return true;
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
            if(puzzleObj.CheckAllRays(this.gameObject))
            {
                DataSaveManager.Instance.UpdateGameState(floorIndex, puzzleObj.getObjectIndex);
                puzzleObj.OffInteraction();
            }
        }
    }
}

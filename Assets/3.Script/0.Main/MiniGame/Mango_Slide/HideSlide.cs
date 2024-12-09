using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSlide : SlideObject
{

    [Header("상호작용이 되어야하는 오브젝트는 true")]
    [SerializeField] private bool isHideObj;

    public int floorIndex;
    [SerializeField] private int objIndex;
    
    private MeshRenderer renderer;
    
    private Color fillColor = Color.white;
    private Color hideColor = Color.clear;
    private Color halfColor = new Color(1f, 1f, 1f, 0.6f);

    private bool isClear;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out renderer);
        isClear = DataSaveManager.Instance.GetGameState(floorIndex, objIndex);
    }

    public bool IsInteracted(int objId, bool touchEnd)
    {
        if(!isClear)
        {
            if (objId.Equals(objIndex))
            {
                if (touchEnd)
                {
                    renderer.material.color = fillColor;
                    isClear = true;
                }
                else
                {
                    renderer.material.color = halfColor;
                }
                return true;
            }
            renderer.material.color = hideColor;
        }
        return false;
    }
    public void HideMaterial()
    {
        renderer.material.color = hideColor;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LerpImage : MonoBehaviour, ITouchable
{
    [SerializeField] public RectTransform inventoryButton; // 인벤토리 버튼의 RectTransform
    

    public void OnTouchStarted(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnTouchHold(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnTouchEnd(Vector2 position)
    {
        throw new System.NotImplementedException();
    }
}

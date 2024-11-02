using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wiring : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private eColor color;
    private bool isCheckColor;
    public bool IsCheckColor { get => isCheckColor; }
    public void SetBoolCheckColor(bool checkcolor)
    {
        isCheckColor = checkcolor;
    }

    private ReadInputData input;

    private void Start()
    {
        TryGetComponent(out input);
    }

    private void Update()
    {
        if (input.isTouch)
        {
            DragConnectWiring();
        }
    }

    public bool WiringSameColorCheck(eColor color)
    {
        if(this.color == color)
        {
            isCheckColor = true;
        }
        else
        {
            isCheckColor = false;
        }

        return isCheckColor;
    }

    public void SetColor(eColor color)
    {
        this.color = color;
    }

    public eColor GetWiringColor()
    {
        return color;
    }

    public void SetWiringColor(int color)
    {
        this.color = (eColor)color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(color);
        // 위치를 옮긴다기보다는 늘이니까, 위치는 움직이지 않음 
        // onenter 되면 터치를 false 하는 식으로 
        // 라인렌더라
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.pointerEnter.gameObject.name == "EndPoint" && eventData.pointerEnter.gameObject.TryGetComponent(out WiringPoint points))
        {
            if(this.color == points.GetWiringColor())
            {
                Debug.Log("색깔 같음");
                isCheckColor = true;
            }
            else
            {
                Debug.Log("색깔 틀림");
            }
        }
    }


    // 
    public void DragConnectWiring()
    {
        //if (inputdata.isTouch)
        //{
        //    //터치가 들어온거임 
        //    //inputdata.value 손가락 따라다니는 밸류   
        //}

        Debug.Log("호출");

    }
}

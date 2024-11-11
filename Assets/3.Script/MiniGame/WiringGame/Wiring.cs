using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Wiring : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private eColor color;
    private bool isCheckColor;
    public bool IsCheckColor { get => isCheckColor; }
    public void SetBoolCheckColor(bool checkcolor)
    {
        isCheckColor = checkcolor;
    }

    private ReadInputData inputdata;
    [SerializeField] LineRenderer linerender;
    [SerializeField] Camera wiriCamera;
    [SerializeField] WiringGameManager wiringGameManager;

    private void Awake()
    {
        TryGetComponent(out inputdata);
        linerender.positionCount = 2;

    }


    public bool WiringSameColorCheck(eColor color)
    {
        if (this.color == color)
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
        linerender.SetPosition(0, transform.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 test = new Vector3(eventData.position.x, eventData.position.y, -1000f);
        Vector3 worldpos = wiriCamera.ScreenToWorldPoint(test);
        linerender.SetPosition(1, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject.name == "EndPoint" && eventData.pointerEnter.gameObject.TryGetComponent(out WiringPoint points))
        {
            linerender.SetPosition(1, eventData.pointerEnter.transform.position);
            if (this.color == points.GetWiringColor())
            {
                Debug.Log("»ö±ò °°À½");
                isCheckColor = true;
                if(eventData.pointerEnter.gameObject.TryGetComponent(out WiringPoint point))
                {
                    point.SetboolConnect(true);
                }
                wiringGameManager.CheckWiringBool();

            }
            else
            {
                Debug.Log("»ö±ò Æ²¸²");

                isCheckColor = false;
            }
        }
    }


    // 
    public void DragConnectWiring()
    {
        if (inputdata.isTouch)
        {
            Debug.Log("È£Ãâ");
            linerender.SetPosition(0, inputdata.startValue);
            linerender.SetPosition(0, inputdata.value);
        }

    }

}

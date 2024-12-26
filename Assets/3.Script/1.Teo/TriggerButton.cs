using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IUseTrigger
{
    public void OnUseTrigger(int id);
}

public class TriggerButton : MonoBehaviour, IUITouchable
{
    public Item item { get; private set; } = null;
    public Image image { get; private set; }

    [SerializeField] private GameObject triggerList;


    [Header("작을 수록 빠름")]
    [Range(0.1f, 2f)]
    [SerializeField] private float touchLength;
    
    private float touchTime;// touchLength까지 도달할 시간

    //트리거버튼을 눌렀을때 트리거가능한 오브젝트가 있다면 메서드 실행
    public static event Action<int> OnUseTrigger; 

    private void InitButton()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        touchTime = 0f;
    }
    public void SetTriggerByItem(Item item)
    {
        if(image == null)
        {
            InitButton();
        }
        this.item = item;
        image.sprite = item.sprite;
    }


    public void OnUIStarted(PointerEventData data)
    {
        if (!triggerList.activeSelf)
            touchTime = 0f;
    }

    public void OnUIHold(PointerEventData data)
    {
        if (!triggerList.activeSelf)
            touchTime += Time.deltaTime;
    }

    public void OnUIEnd(PointerEventData data)
    {
        if (!triggerList.activeSelf)
        {
            if (touchTime > touchLength)
            {

                triggerList.SetActive(true);


            }
            else
            {
                if (!triggerList.activeSelf)
                    OnUseTrigger?.Invoke(item.id);
            }
            touchTime = 0f;
        }
    }
}

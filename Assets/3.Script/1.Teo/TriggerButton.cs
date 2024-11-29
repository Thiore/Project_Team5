using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IUseTrigger
{
    public void OnUseTrigger(Item item);
}

public class TriggerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item item { get; private set; } = null;
    private Image image;

    [SerializeField] private GameObject triggerList;


    [Header("작을 수록 빠름")]
    [Range(0.1f, 2f)]
    [SerializeField] private float touchLength;
    
    private float touchTime;// touchLength까지 도달할 시간

    private Coroutine touchTime_co;

    //트리거버튼을 눌렀을때 트리거가능한 오브젝트가 있다면 메서드 실행
    public static event Action<Item> OnUseTrigger; 


    private void Start()
    {
        if (image == null)
        {
            InitButton();
        }
    }

    private void InitButton()
    {
        transform.GetChild(0).TryGetComponent(out image);
        touchTime = 0f;
        touchTime_co = null;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!triggerList.activeSelf)
            touchTime_co = StartCoroutine(TouchTime_co());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!triggerList.activeSelf)
        {
            StopCoroutine(touchTime_co);
            touchTime_co = null;
            if (touchTime > touchLength)
            {

                triggerList.SetActive(true);


            }
            else
            {
                if (!triggerList.activeSelf)
                    OnUseTrigger?.Invoke(item);
            }
            touchTime = 0f;
        }
    }
    private IEnumerator TouchTime_co()
    {
        while(true)
        {
            touchTime += Time.deltaTime;
            yield return null;
        }
        
    }
}

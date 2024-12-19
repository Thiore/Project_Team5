using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerList : MonoBehaviour
{
    [SerializeField] private TriggerButton button;
    public TriggerButton getTriggerButton { get => button; }

    public Coroutine openList_co { get; private set; } = null;

    private HorizontalLayoutGroup layoutGroup;
    private RectTransform rect;
    [Header("작을 수록 빠름")]
    [Range(0.1f,2f)]
    [SerializeField] private float lerpSpeed;

    float lerpTime;

    public bool isOpen { get; private set; }

    private void OnEnable()
    {
        if (layoutGroup == null||rect == null)
        {
            TryGetComponent(out layoutGroup);
            TryGetComponent(out rect);
        }
        layoutGroup.spacing = -100f;
        isOpen = true;
        lerpTime = 0f;
        openList_co = StartCoroutine(SetList_co());
    }
    private void OnDisable()
    {
        lerpTime = 0f;
        StopCoroutine(openList_co);
        openList_co = null;

    }
    public void CloseList()
    {
        isOpen = false;
        if (openList_co != null)
            StopCoroutine(openList_co);
        openList_co = StartCoroutine(SetList_co());
    }
    private IEnumerator SetList_co()
    {
        while(true)
        {
            if (isOpen)
            {

                lerpTime += Time.deltaTime* lerpSpeed;
                float listX = Mathf.Lerp(50f, -100f, Mathf.Clamp(lerpTime, 0f, 1f));
                rect.anchoredPosition = new Vector2(listX, rect.anchoredPosition.y);
                layoutGroup.spacing = Mathf.Lerp(-100f, 30f, Mathf.Clamp(lerpTime, 0f, 1f));
                if (lerpTime >= 1f)
                {
                    openList_co = null;
                    yield break;
                }
                yield return null;
            }
            else
            {

                lerpTime -= Time.deltaTime* lerpSpeed;
                float listX = Mathf.Lerp(50f, -100f, Mathf.Clamp(lerpTime, 0f, 1f));
                rect.anchoredPosition = new Vector2(listX, rect.anchoredPosition.y);
                layoutGroup.spacing = Mathf.Lerp(-100f, 30f, Mathf.Clamp(lerpTime, 0f, 1f));
                if (lerpTime <= 0f)
                {
                    gameObject.SetActive(false);
                }
                yield return null;
                
                

            }

            
        }
        

    }
}

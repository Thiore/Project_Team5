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

    public bool isOpen { get; private set; } = false;

    private void OnEnable()
    {
        if (layoutGroup == null||rect == null)
        {
            TryGetComponent(out layoutGroup);
            TryGetComponent(out rect);
        }
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
        openList_co = StartCoroutine(SetList_co());
    }
    private IEnumerator SetList_co()
    {
        lerpTime += Time.deltaTime;
        if (isOpen)
        {
            while (lerpTime / lerpSpeed < 1f)
            {
                float listX = Mathf.Lerp(50f, -100, lerpTime / lerpSpeed);
                rect.anchoredPosition = new Vector2(listX, rect.anchoredPosition.y);
                layoutGroup.spacing = Mathf.Lerp(-100f, 30f, lerpTime / lerpSpeed);
                yield return null;
            }
            lerpTime = 0f;
            openList_co = null;
            yield break;
        }
        else
        {
            while (lerpTime / lerpSpeed < 1f || !isOpen)
            {
                float listX = Mathf.Lerp(-100, 50f, lerpTime / lerpSpeed);
                rect.anchoredPosition = new Vector2(listX, rect.anchoredPosition.y);
                layoutGroup.spacing = Mathf.Lerp(30f, -100f, lerpTime / lerpSpeed);
                yield return null;
                
            }
        }
        
        gameObject.SetActive(false);

    }
}

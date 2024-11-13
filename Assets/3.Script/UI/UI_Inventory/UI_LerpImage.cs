using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LerpImage : MonoBehaviour, ITouchable
{
    [SerializeField] public RectTransform inventoryButton; // 인벤토리 버튼의 RectTransform
    private Vector2 startPosition; // 출발 위치
    private Vector2 endPosition;   // 도착 위치 (인벤토리 버튼 위치)
    private float lerpTime = 0f;   // Lerp 진행도
    private bool isLerping = false;

    private RectTransform rectTransform; // 러프이미지 RectTransform

    private void Awake()
    {
        endPosition = inventoryButton.anchoredPosition;
    }

    public void StartLerp(Vector3 objectWorldPosition)
    {
        // 3D 오브젝트의 월드 좌표를 화면 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(objectWorldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform, screenPosition, null, out startPosition
        );

        rectTransform.anchoredPosition = startPosition;
        lerpTime = 0f;
        isLerping = true;
    }

    private void Update()
    {
        if (isLerping)
        {
            lerpTime += Time.deltaTime * 2f; // 속도 조정 가능
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, lerpTime);

            if (lerpTime >= 1f)
            {
                isLerping = false;
                lerpTime = 0f;
            }
        }
    }

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

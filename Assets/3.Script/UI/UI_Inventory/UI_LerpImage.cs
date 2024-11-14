using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LerpImage : MonoBehaviour
{
    [SerializeField]private RectTransform inventoryButton; // 인벤토리 버튼의 RectTransform
    [SerializeField]private Canvas canvas;
    
    private Vector2 invenPos;

    private RectTransform lerpImage;

    public Coroutine lerp_co { get; private set; } = null;

    public void Start()
    {
        TryGetComponent(out lerpImage);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            inventoryButton.anchoredPosition,
            null,
            out invenPos);
    }

    //아이템의 터치앤드에서 이 함수 불러주시면 됩니다!
    public void OnLerpItem(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            position,
            null,
            out Vector2 localPoint);

        if(lerp_co == null)
        {
            lerp_co = StartCoroutine(LerpImage_co(localPoint));
        }
    }

    private IEnumerator LerpImage_co(Vector2 localPoint)
    {
        float lerpTime = 0f;
        while(lerpTime.Equals(1f))
        {
            lerpTime += Time.deltaTime;
            if(lerpTime>1f)
            {
                lerpTime = 1f;
            }
            lerpImage.anchoredPosition = Vector2.Lerp(localPoint, invenPos, lerpTime);
            if(!lerpImage.gameObject.activeSelf)
            {
                lerpImage.gameObject.SetActive(true);
            }
            yield return null;
        }
        //여기서 인벤에 들어가는 처리하면 될것같습니다!
        lerpImage.gameObject.SetActive(false);
        lerp_co = null;
        yield break;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LerpImage : MonoBehaviour
{
    [SerializeField] public RectTransform inventoryButton; // 인벤토리 버튼의 RectTransform
    private Image image;

    private void OnEnable()
    {
        TryGetComponent(out image);
    }

    private void OnDisable()
    {
        StopCoroutine(MoveInvenButton_co());
    }

    public void InputMovementInventory(Item item, Vector2 pos)
    {
        transform.position = pos;
        image.sprite = item.Sprite;

        StartCoroutine(MoveInvenButton_co());

    }


    private IEnumerator MoveInvenButton_co()
    {
        float lerptiem = 0f;
        Vector2 startpos = transform.position;
        Vector2 tartgetpos = inventoryButton.transform.position;

        while (lerptiem * 1.2f < 1f)
        {
            lerptiem += Time.fixedDeltaTime;

            transform.position = Vector3.Lerp(startpos, tartgetpos, lerptiem  * 1.2f);

            yield return null;
        }
        gameObject.SetActive(false);

    }


}

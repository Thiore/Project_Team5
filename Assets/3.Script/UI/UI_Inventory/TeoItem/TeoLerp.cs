using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeoLerp : MonoBehaviour
{
    [SerializeField] private RectTransform inventoryButton; // �κ��丮 ��ư�� RectTransform
    [SerializeField] private Inventory inven;
    private Image image;
    public Coroutine lerp_co;

    

    private void Start()
    {
        lerp_co = null;
    }

    public bool InputMovementInventory(TeoItemData item, Vector2 pos)
    {
        if(lerp_co == null)
        {
            gameObject.SetActive(true);
            if(image == null)
            {
                TryGetComponent(out image);
            }
            transform.position = pos;
            image.sprite = item.sprite;
            lerp_co = StartCoroutine(MoveInvenButton_co(item));
            return true;
        }
        return false;
       

    }


    private IEnumerator MoveInvenButton_co(TeoItemData item)
    {
        float lerptiem = 0f;
        Vector2 startpos = transform.position;
        Vector2 tartgetpos = inventoryButton.transform.position;
        
        while (lerptiem * 1.2f < 1f)
        {
            lerptiem += Time.fixedDeltaTime;

            transform.position = Vector3.Lerp(startpos, tartgetpos, lerptiem * 1.2f);

            yield return null;
        }
        
        inven.GetItem(item);
        lerp_co = null;
        gameObject.SetActive(false);
        yield break;
    }
}

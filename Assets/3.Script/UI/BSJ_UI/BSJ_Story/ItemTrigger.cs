using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    // 자신의 인덱스 Item
    [SerializeField] private int itemExplanationIndex;

    private ReadInputData input;

    private void Start()
    {
        //ReadInputData 가져오기
        TryGetComponent(out input);
    }

    private void Update()
    {
        ItemText();
    }

    //상호작용 독백 출력
    private void ItemText()
    {
        if (gameObject.activeSelf && input.isTouch)
        {
            DialogueManager.Instance.SetDialogue("Table_ItemExplanation", itemExplanationIndex);
        }
    }
}

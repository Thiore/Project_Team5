using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* public enum ConnectionColor
{
    Red,
    Blue,
    Gray,
    Pink,
    Orange,
    Yellow,
    Sky_Blue,
} */

public class Mission : MonoBehaviour
{
    [Header("Line 어태치")]
    [SerializeField] private GameObject obj = null;
    [SerializeField] private LineRenderer lr = null;

    // [Header("Box 어태치")]
    // [SerializeField] private Transform[] box = null;

    private bool ClickState = false;

    // [SerializeField] private ConnectionColor color;

    public void ResetButton()
    {
        Debug.Log("리셋!");
    }

    public void TutorialButton()
    {
        Debug.Log("튜토리얼!");
    }

    public void CloseButton()
    {
        Debug.Log("나가기!");
    }

    public void ChangeColor(Material color)
    {
        lr.material = color;
    }

    public void StartDrag(GameObject pos)
    {
        if (!lr.enabled)
        {
            lr.gameObject.transform.SetParent(pos.transform);

            lr.gameObject.transform.localPosition = new Vector3(0.6f, 0, 0);

            Debug.Log("활성화!");

            lr.enabled = true;
        }
    }

    public void EndDrag()
    {
        Debug.Log("비활성화!");

        lr.enabled = false;
    }

    public void Box()
    {
        int count = lr.positionCount;

        count += 1;

        // 추가 수정 필요...
    }
}
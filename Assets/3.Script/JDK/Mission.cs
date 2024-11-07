using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Mission : MonoBehaviour
{
    [Header("Line 어태치")]
    [SerializeField] private LineRenderer lr = null;

    [Header("줄 프로퍼티")]
    [SerializeField] private float LineValue = 0; // 1.2

    private Vector2 startDragPosition;
    private bool RightLeftState = false;
    private bool UpDownState = false;

    public void ResetButton()
    {
        Debug.Log("리셋!");
    }

    public void TutorialButton()
    {
        Debug.Log("튜토리얼!");
    }

    public void ChangeColor(Material color)
    {
        lr.material = color;
    }

    public void ResetLine()
    {
        EndDrag();
    }

    public void StartDrag(GameObject pos)
    {
        if (!lr.enabled)
        {
            lr.gameObject.transform.SetParent(pos.transform);

            lr.gameObject.transform.localPosition = new Vector3(0.6f, 0, 0);
            lr.gameObject.transform.localScale = new Vector3(1, 1, 1);

            lr.enabled = true;
        }
    }

    public void EndDrag()
    {
        lr.positionCount = 1;

        lr.enabled = false;
    }

    public void Line(string sentence)
    {
        int count = lr.positionCount;

        Vector3 pos = lr.GetPosition(count - 1);

        switch (sentence)
        {
            case "RL":

                if (!UpDownState)
                {
                    lr.positionCount = count + 1;

                    pos.z += LineValue;

                    lr.SetPosition(count, pos);

                    Debug.Log("오른쪽!");
                }
                else
                {
                    lr.positionCount = count + 1;

                    pos.z -= LineValue;

                    lr.SetPosition(count, pos);

                    Debug.Log("왼쪽!");

                    UpDownState = false;
                }

                break;

            case "UD":

                UpDownState = true;

                lr.positionCount = count + 1;

                pos.y -= LineValue;

                lr.SetPosition(count, pos);

                Debug.Log("위! 아래!");

                break;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractionKeyPad : TouchPuzzleCanvas
{
    [SerializeField] private InputField input;
    [SerializeField] private int answer;
    public override void OffInteraction()
    {
        base.OffInteraction();

        mask.enabled = true;
        missionStart.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(false);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(false);
        }
    }
    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                mask.enabled = false;
                missionStart.SetActive(true);
                btnExit.SetActive(true);
                TouchManager.Instance.EnableMoveHandler(false);
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.SetBtn(false);
                }
            }
        }
    }
    public void BtnClick(Button clickedBtn)
    {
        string btnName = clickedBtn.name;
        Text btnText = clickedBtn.GetComponentInChildren<Text>();

        if (int.TryParse(btnText.text, out int result))
        {
            if (input.text.Length >= 4)
            {
                Debug.Log("Too many input");
                return;
            }
            input.text += btnText.text;
        }
        else
        {
            if (btnText.text.Equals("Enter"))
            {
                Debug.Log($"Answer is {input.text}");
                if (input.text.Equals(answer.ToString()))
                {
                    DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
                    isClear = true;
                    OffInteraction();
                }
                input.text = "";
                
            }
            else
            {
                input.text = input.text.Substring(0, input.text.Length - 1);
            }
        }
    }

    protected override void ClearEvent()
    {
    }

    protected override void ResetCamera()
    {
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (isClear) return;
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }
    }
}

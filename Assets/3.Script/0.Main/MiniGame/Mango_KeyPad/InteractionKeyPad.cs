using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InteractionKeyPad : TouchPuzzleCanvas
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private int answer;
    [SerializeField] private int limitLength;
    public override void OffInteraction()
    {
        base.OffInteraction();

        missionStart.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        mask.enabled = true;
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
    public void BtnClick(TMP_Text clickedBtn)
    {
        if (int.TryParse(clickedBtn.text, out int result))
        {
            if (input.text.Length >= limitLength)
            {
                //Debug.Log("Too many input");
                return;
            }
            input.text += clickedBtn.text;
        }
        else
        {
            if (clickedBtn.text.Equals("ENTER"))
            {
                //Debug.Log($"Answer is {input.text}");
                if (input.text.Equals(answer.ToString()))
                {
                    DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
                    isClear = true;
                    input.text = "Clear";
                    OffInteraction();
                    return;
                }
                else
                {
                    input.text = string.Empty;
                }
                
                
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

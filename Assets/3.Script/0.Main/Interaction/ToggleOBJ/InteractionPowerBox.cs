using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPowerBox : TouchPuzzleCanvas
{
    public StartPoint[] startArray;
    public EndPoint[] endArray;
    private WireColor[] colors = { WireColor.Red, WireColor.Orange, WireColor.Green, WireColor.Yellow };

    protected override void OnEnable()
    {
        base.OnEnable();
        if(!isClear)
        {
            foreach (var start in startArray)
            {
                start.CheckConnecting += CheckWireConnect;
            }
            ShuffleArray(startArray);
            ShuffleArray(endArray);
            AssignRandomColors(startArray, endArray);
        }
        
    }

    public override void OffInteraction()
    {
        base.OffInteraction();
        if (!isClear)
        {
            mask.enabled = true;
            outline.enabled = true;
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
            missionStart.SetActive(false);
            missionExit.SetActive(false);
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.SetBtn(true);
            }
            TouchManager.Instance.EnableMoveHandler(true);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
            }
        }
        else
        {
            
            missionStart.SetActive(false);
            if (anim != null)
            {
                anim.SetBool(openAnim, false);
            }
            Invoke("ClearEvent", 2f);
        }
    }

    protected override void ClearEvent()
    {
        for (int i = 0; i < interactionAnim.Length; i++)
        {
            interactionAnim[i].SetBool(openAnim, true);
        }
        Invoke("ResetCamera", 1f);
    }
    protected override void ResetCamera()
    {
        missionExit.SetActive(false);
        TouchManager.Instance.EnableMoveHandler(true);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
    }
    private void CheckWireConnect()
    {
        foreach (var start in startArray)
        {
            if (!start.isConnect)
            {
                Debug.Log("아직 성공아님");
                return;
            }
        }

        Debug.Log("게임성공");
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
        isClear = true;
        OffInteraction();
        Debug.Log("나와라");
    }

    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (!isInteracted && !UI_InvenManager.Instance.HaveItem(interactionIndex))
                {
                    DialogueManager.Instance.SetDialogue("Table_StoryB1", 12);
                }
                else if (!isInteracted)
                {
                    if (missionStart.activeInHierarchy)
                    {
                        DialogueManager.Instance.SetDialogue("Table_StoryB1", 0);
                    }
                    else
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);
                        outline.enabled = false;
                        if (PlayerManager.Instance != null)
                        {
                            PlayerManager.Instance.SetBtn(false);
                        }
                        UI_InvenManager.Instance.OpenQuickSlot();
                    }
                }
                else
                {
                    if (!missionStart.activeInHierarchy)
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);
                        if (PlayerManager.Instance != null)
                        {
                            PlayerManager.Instance.SetBtn(false);
                        }
                        mask.enabled = false;
                        outline.enabled = false;
                    }
                    if (UI_InvenManager.Instance.isOpenQuick)
                    {
                        UI_InvenManager.Instance.CloseQuickSlot();
                    }
                    if (anim != null)
                    {
                        anim.SetBool(openAnim, true);
                       
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClear) return;


        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }

    }

    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1); // 현재 인덱스까지의 무작위 인덱스를 선택
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // 배열의 각 요소에 무작위 색상 할당
    private void AssignRandomColors(StartPoint[] startArray, EndPoint[] endArray)
    {
        // colors 배열을 무작위로 섞음
        ShuffleArray(colors);

        // 배열의 각 요소에 무작위 색상 할당
        for (int i = 0; i < startArray.Length; i++)
        {
            startArray[i].setColor(colors[i]); // 섞인 colors 배열의 색상을 순서대로 할당
            startArray[i].setMaterial();
            endArray[i].setColor(colors[i]);
            endArray[i].setMaterial();
        }
    }
}

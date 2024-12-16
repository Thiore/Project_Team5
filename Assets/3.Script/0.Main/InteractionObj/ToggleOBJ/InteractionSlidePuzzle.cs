using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSlidePuzzle : TouchPuzzleCanvas
{
    public GameObject selectedObject { get; private set; } // 현재 터치된 오브젝트

    [SerializeField] private Transform correctZone;
    [SerializeField] private Transform cubeParents;
    private float rayDistance =0.2f; // Ray의 길이

    private bool isTouching;
    LayerMask checkLayer;

    [SerializeField]
    private float rayOffset;// Ray 시작 위치의 오프셋 배열을 인스펙터에서 설정 가능하게 함
    private Vector3[] rayOffsets = new Vector3[4];

    private void Awake()
    {
        for (int i = 0; i < 4; ++i)
        {
            switch (i)
            {
                case 0:
                    rayOffsets[0] = correctZone.position - correctZone.forward * rayOffset - correctZone.right * rayOffset;
                    break;
                case 1:
                    rayOffsets[1] = correctZone.position - correctZone.forward * rayOffset + correctZone.right * rayOffset;
                    break;
                case 2:
                    rayOffsets[2] = correctZone.position + correctZone.forward * rayOffset - correctZone.right * rayOffset;
                    break;
                case 3:
                    rayOffsets[3] = correctZone.position + correctZone.forward * rayOffset + correctZone.right * rayOffset;
                    break;
            }
        }
        checkLayer = LayerMask.GetMask("SlideObject");
        isTouching = false;
    }

    public bool SetSelectObj(GameObject selected)
    {
        if (selectedObject != null)
        {
            return false;
        }
        else
        {
            selectedObject = selected;
            return true;
        }
    }
    public void ResetSelectObj()
    {
        selectedObject = null;
    }
    public void ResetPuzzle()
    {
        for(int i = 0; i < cubeParents.transform.childCount;++i)
        {
            cubeParents.transform.GetChild(i).TryGetComponent(out SlideObject obj);
            obj.InitPosition();
        }
    }
    public void CheckAllRays(GameObject checkObj)
    {
        
        foreach (Vector3 offset in rayOffsets)
        {
            // y 방향으로 Ray 발사
            if (Physics.Raycast(offset, correctZone.up, out RaycastHit hit, rayDistance, checkLayer))
            {
                // 충돌한 오브젝트와 일치하지 않으면 false 반환
                if (!hit.collider.gameObject.Equals(checkObj))
                {
                    Debug.Log($"Ray from {offset} did not hit target tag.");
                    return;
                }
            }
            else
            {
                // Ray가 아무 오브젝트와도 충돌하지 않으면 false 반환
                Debug.Log($"Ray from {offset} did not hit anything.");
                return;
            }
        }

        // 모든 Ray가 지정된 태그의 오브젝트와 충돌하면 true 반환
        DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex, true);
        isClear = true;
        btnExit.SetActive(false);
        OffInteraction();
        return;
    }



    public override void OffInteraction()
    {
        base.OffInteraction();
        if (!isClear)
        {
            mask.enabled = true;
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
        }
        else
        {
            //slidePuzzle.SetParent(transform);
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
        if (anim != null)
        {
            anim.SetBool(openAnim, true);
        }
        Invoke("ResetCamera", 1f);
    }

    protected override void ResetCamera()
    {
        mask.enabled = true;
        missionExit.SetActive(false);
        isTouching = true;
        interactionCam.SetActive(true);
    }

    public override void InteractionObject(int id)
    {
        base.InteractionObject(id);
        if(isInteracted)
        {
            mask.enabled = false;
        }
    }

    public override void OnTouchEnd(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if(!isClear)
                {
                    if (!isInteracted)
                    {
                        if (!UI_InvenManager.Instance.HaveItem(interactionIndex))
                        {
                            DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
                            //Dialogue - 퍼즐이 필요합니다?
                            return;
                        }
                        if (missionStart.activeInHierarchy)
                        {
                            DialogueManager.Instance.SetDialogue("Table_StoryB1", 1);
                            //Dialogue - 퍼즐을 다 넣어야한다는거?
                        }
                        else
                        {
                            TouchManager.Instance.EnableMoveHandler(false);
                            missionStart.SetActive(true);
                            missionExit.SetActive(true);
                            btnExit.SetActive(true);
                            if (PlayerManager.Instance != null)
                            {
                                PlayerManager.Instance.SetBtn(false);
                            }
                            UI_InvenManager.Instance.OpenQuickSlot();
                        }
                    }
                    else
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                        missionStart.SetActive(true);
                        missionExit.SetActive(true);
                        btnExit.SetActive(true);

                        mask.enabled = false;
                        if (PlayerManager.Instance != null)
                        {
                            PlayerManager.Instance.SetBtn(false);
                        }

                        if (UI_InvenManager.Instance.isOpenQuick)
                        {
                            UI_InvenManager.Instance.CloseQuickSlot();
                        }
                    }
                }
                else
                {
                    isTouching = !isTouching;
                    
                    interactionCam.SetActive(isTouching);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(!isTouching);
                    }
                    TouchManager.Instance.EnableMoveHandler(!isTouching);


                    if (anim != null)
                    {
                        anim.SetBool(openAnim, isTouching);
                    }
                }
               
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        // Gizmos를 사용해 Scene View에서 Ray를 시각적으로 확인
        Gizmos.color = Color.green;
        foreach (Vector3 offset in rayOffsets)
        {
            Gizmos.DrawRay(offset, correctZone.up * rayDistance);
        }
    }
#endif
    private void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}

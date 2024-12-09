using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOBJ : InteractionOBJ, ITouchable
{
    [Header("DataSaveManager 참고")]
    [SerializeField] private int floorIndex;
    [SerializeField] private int objectIndex;

    [SerializeField] private int storyIndex;

    private bool isInteracted = true;

    [Header("퍼즐 또는 퀵슬롯의 다른오브젝트와 상호작용이 필요하면 False")]
    [SerializeField] private bool isClear;

    private Coroutine closeCam_co = null;

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        TryGetComponent(out anim);
        if (!isClear)
        {
            isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);
            isInteracted = false;
        }
    }

    public void OnTouchStarted(Vector2 position)
    {
    }
    public void OnTouchHold(Vector2 position)
    {

    }
    public void OnTouchEnd(Vector2 position)
    {
        if(closeCam_co == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
            {
                if (hit.collider.gameObject.Equals(gameObject))
                {
                    isClear = DataSaveManager.Instance.GetGameState(floorIndex, objectIndex);

                    if (isClear)
                    {
                        if (normalCamera != null)
                        {
                            if (!normalCamera.activeInHierarchy || isInteracted)
                            {
                                isTouching = !isTouching;
                                normalCamera.SetActive(isTouching);
                                anim.SetBool(openAnim, isTouching);
                            }
                            else
                            {
                                isInteracted = true;
                                anim.SetBool(openAnim, isTouching);
                            }

                        }
                    }
                    else
                    {
                        if (normalCamera != null)
                        {
                            if (!normalCamera.activeInHierarchy)
                            {
                                normalCamera.SetActive(true);
                                isTouching = true;
                            }
                            else
                            {
                                //"잠겨있어"라는 독백 대사 출력
                                DialogueManager.Instance.SetDialogue("Table_StoryB1", storyIndex);
                                closeCam_co = StartCoroutine(CloseInteractionCam_co());
                            }

                        }
                    }
                }
            }
        }
        
    }
    
    private IEnumerator CloseInteractionCam_co()
    {
        while(DialogueManager.Instance.isDialogue)
        {
            yield return null;
        }
        normalCamera.SetActive(false);
        isTouching = false;
        closeCam_co = null;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSpinPuzzle : TouchPuzzleCanvas
{
    private List<SpinTile> tileList;
    [SerializeField] private Transform tileParents;
    [SerializeField] private MeshRenderer[] lamps;
    private Material[] lampMaterials;

    [SerializeField] private List<SpinTile> targetObjects; // 모든 타겟 오브젝트 리스트
 
    
    private List<SpinTile> connectedObjects;

    [SerializeField] private FixPipeGameManager pipeManager;
    

    protected override void Awake()
    {
        base.Awake();
        lampMaterials = new Material[lamps.Length];
        tileList = new List<SpinTile>();
        connectedObjects = new List<SpinTile>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

    private void Start()
    {
        if (isClear)
        {
            
            mask.enabled = false;
            return;
        }

        
        // 모든 타일에 있는 SpinTile의 회전 완료 이벤트 구독
        for(int i = 0; i < tileParents.childCount;++i)
        {
            tileParents.GetChild(i).TryGetComponent(out SpinTile tile);
            if (tile != null)
            {
                tileList.Add(tile);
                tile.OnRotationComplete += CheckRay; // 회전 완료 시 CheckRay 호출
            }
        }
        for (int i = 0; i < lamps.Length; ++i)
        {
            lampMaterials[i] = lamps[i].material;
            lampMaterials[i].DisableKeyword("_EMISSION");
        }

    }

    // CheckRay 메서드: 모든 rayStartTile의 RayCheck를 확인
    private void CheckRay()
    {
        if (CheckConnections())
        {
            foreach (Material lamp in lampMaterials)
                lamp.EnableKeyword("_EMISSION");
            DataSaveManager.Instance.UpdateGameState(floorIndex, objectIndex);
            isClear = true;
            OffInteraction();
        }

    }

    // 연결 상태를 확인하는 메서드
    public bool CheckConnections()
    {
        if (targetObjects[0] == null)
        {
            //Debug.LogError("Starting SpinTile is not assigned!");
            return false;
        }

        connectedObjects.Clear(); // 클래스 변수 연결 목록을 초기화
        RecursiveConnectionCheck(targetObjects[0], null, connectedObjects);

        // 시작 타일의 연결을 확인
        return AreAllTargetsConnected(connectedObjects);
    }

    // 재귀적으로 연결 상태 확인 (양방향 연결 필수)
    private void RecursiveConnectionCheck(SpinTile SpinTile, GameObject previousObject, List<SpinTile> connectedObjects)
    {
        connectedObjects.Add(SpinTile); // 현재 오브젝트를 연결 목록에 추가

        List<SpinTile> hitObjects = SpinTile.GetHitObject(); // 현재 오브젝트가 감지한 연결된 오브젝트 목록

        foreach (var hitObject in hitObjects)
        {
            //Debug.Log($"hit object : {hitObject.name}");
            // 현재 hitObject가 이전 오브젝트와의 양방향 연결을 확인
            SpinTile hitSpinTile = hitObject.GetComponent<SpinTile>();

            if (hitSpinTile != null && !connectedObjects.Contains(hitObject))
            {
                // hitObject가 이전 오브젝트와 연결이 되어 있는지 확인
                if (previousObject == null || IsMutuallyConnected(hitSpinTile, SpinTile))
                {
                    RecursiveConnectionCheck(hitSpinTile, SpinTile.gameObject, connectedObjects);
                }
            }
        }
    }

    // 양방향 연결 확인
    private bool IsMutuallyConnected(SpinTile SpinTile, SpinTile targetObject)
    {
        List<SpinTile> hitObjects = SpinTile.GetHitObject();
        return hitObjects.Contains(targetObject);
    }

    // 연결된 오브젝트가 모든 타겟 오브젝트와 일치하는지 확인
    private bool AreAllTargetsConnected(List<SpinTile> connectedObjects)
    {
        foreach (var target in targetObjects)
        {
            if (!connectedObjects.Contains(target))
            {
                //Debug.Log("Not all target objects are connected.");
                return false; // 타겟 오브젝트 중 연결되지 않은 것이 있음
            }
        }

        //Debug.Log("All target objects are successfully connected!");
        this.connectedObjects = connectedObjects;
        return true; // 모든 타겟 오브젝트가 연결됨
    }

    public override void OffInteraction()
    {
        base.OffInteraction();
        
        if (!isClear)
        {
            missionStart.SetActive(false);
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.SetBtn(true);
            }
            TouchManager.Instance.EnableMoveHandler(true);
            mask.enabled = true;
        }
        
        
        if (anim != null)
        {
            anim.SetBool(openAnim, false);
        }
        if(isClear)
        {
            mask.enabled = false;
            outline.enabled = false;
            interactionCam.SetActive(true);
            missionStart.SetActive(false);
            Invoke("ClearEvent", 3f);
        }

    }
    protected override void ClearEvent()
    {
        interactionAnim[0].SetTrigger("Panel");

        Invoke("ResetCamera", 2f);
    }

    protected override void ResetCamera()
    {
        interactionCam.SetActive(false);

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
    }
    public override void OnTouchEnd(Vector2 position)
    {
        if (isClear) return;

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (!isInteracted)
                {
                    if(UI_InvenManager.Instance.HaveItem(interactionIndex))
                    {
                        if (interactionIndex.Count.Equals(1) && interactionIndex[0] < 100)
                        {
                            anim.SetBool(openAnim, true);
                            UI_InvenManager.Instance.OpenQuickSlot();
                        }
                        else
                        {
                            //퓨즈퍼즐을 먼저 풀어야한다.
                        }
                    }
                    else
                    {
                        //필요한 아이템이 있을것같아
                        return;
                    }
                   
                }
                else
                {
                    anim.SetBool(openAnim, true);
                }
                if (!missionStart.activeInHierarchy)
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
    }

    public override void InteractionObject(int id)
    {
        base.InteractionObject(id);
        
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

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
    public bool isComplete { get; private set; }
    
    private List<SpinTile> connectedObjects;

    [SerializeField] private GameObject clearCam;
    [SerializeField] private GameObject engineRoomCam;
    [SerializeField] private GameObject monitorCam;
    [SerializeField] private FixPipeGameManager pipeManager;
    

    private void Awake()
    {
        lampMaterials = new Material[lamps.Length];
        tileList = new List<SpinTile>();
        connectedObjects = new List<SpinTile>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

    protected override void Start()
    {
        if (isClear)
        {
            if (DataSaveManager.Instance.GetGameState(floorIndex, pipeManager.getObjectIndex))
            {
                return;
            }
            else
            {
                TouchManager.Instance.EnableMoveHandler(false);
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.SetBtn(false);
                }
                Invoke("OffInteraction", 2f);
                return;
            }
        }
        base.Start();

        
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
            Debug.LogError("Starting SpinTile is not assigned!");
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
            Debug.Log($"hit object : {hitObject.name}");
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

        Debug.Log("All target objects are successfully connected!");
        this.connectedObjects = connectedObjects;
        return true; // 모든 타겟 오브젝트가 연결됨
    }

    public override void OffInteraction()
    {
        base.OffInteraction();

        if (!isClear)
        {
            if (UI_InvenManager.Instance.isOpenQuick)
            {
                UI_InvenManager.Instance.CloseQuickSlot();
            }
            mask.enabled = true;
        }
        missionStart.SetActive(false);
        
        if (anim != null)
        {
            anim.SetBool(openAnim, false);
        }
        if(isClear)
        {
            if (!interactionAnim[1].GetBool(openAnim))
                interactionAnim[1].SetBool(openAnim, true);

            mask.enabled = false;
            clearCam.SetActive(true);
            engineRoomCam.SetActive(true);
            monitorCam.SetActive(true);

            Invoke("ClearEvent", 2f);
        }

    }
    protected override void ClearEvent()
    {
        interactionAnim[0].SetBool(openAnim, true);
        pipeManager.GameStart();
        Invoke("NextCamMove", 2f);
    }
    private void NextCamMove()
    {
        clearCam.SetActive(false);
        
        Invoke("EngineRoomMove", 2f);
    }
    private void EngineRoomMove()
    {
        interactionAnim[2].SetBool(openAnim, true);
        engineRoomCam.SetActive(false);
        
        Invoke("ResetCamera", 5f);
    }

    protected override void ResetCamera()
    {
        PlayerManager.Instance.resetCam.SetActive(true);
        monitorCam.SetActive(false);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
        PlayerManager.Instance.ResetCamOff();
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
                    missionStart.SetActive(true);
                    btnExit.SetActive(true);
                    TouchManager.Instance.EnableMoveHandler(false);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(false);
                    }
                    mask.enabled = false;

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

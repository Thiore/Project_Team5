using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour, ITouchable, IUseTrigger
{
    public static Tablet Instance { get; private set; }
    [SerializeField] private Transform pivot;
    private Transform playerCam;
    private MeshRenderer render;
    [SerializeField] private Item3D clueTablet;
    [Range(10f,50f)]
    [SerializeField] private float enabledSpeed;

    
    [SerializeField] private int floorIndex;
    [SerializeField] private int spinIndex;
    [SerializeField] private int pipeIndex;
    [SerializeField] private TabletMonitor monitorCanvas;
    [SerializeField] private GameObject Logo;
    private GameObject monitor;

    
    public bool isTablet { get; private set; }


    private bool isGet;
    private bool isOn;

    private float rotTime;
    private Coroutine rotTablet_co = null;

    private BoxCollider col;
    private Outline outline;
    
    private void Awake()
    {
        Instance = this;
        monitor = transform.GetChild(0).gameObject;
        TryGetComponent(out render);
        TryGetComponent(out col);
        TryGetComponent(out outline);
        this.playerCam = PlayerManager.Instance.playerCam;
        rotTime = 1f;
        isOn = true;

        if (DataSaveManager.Instance.GetItemState(clueTablet.ID))
        {
            col.enabled = false;
            isGet = true;
        } 
        else
        {
            isGet = false;
            isTablet = false;
        }
        outline.enabled = false;
        
    }

    private void Start()
    {
        if(isGet)
        {
            transform.SetParent(pivot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one * 2f;
            monitor.SetActive(true);
            SetUseTablet();
            
            if (!DataSaveManager.Instance.GetGameState(floorIndex, pipeIndex))
            {
                Logo.SetActive(true);
                isTablet = true;                
            }
            else
            {
                OnUseTrigger(clueTablet.ID);
                monitorCanvas.gameObject.SetActive(true);
                //monitorCanvas.SetDialogueIndex(100, 117, true);
                render.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (isGet)
        {
            TriggerButton.OnUseTrigger -= OnUseTrigger;
        }

    }

    private IEnumerator SetTablet_co()
    {
        transform.SetParent(pivot);
        float delayTime = 0;
        while(true)
        {
            delayTime += Time.fixedDeltaTime;
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, Vector3.zero, delayTime);
            transform.localRotation = Quaternion.SlerpUnclamped(transform.localRotation, Quaternion.identity, delayTime);
            transform.localScale = Vector3.LerpUnclamped(transform.localScale, Vector3.one * 2f, delayTime);
            if (delayTime>=1f)
            {
                SetUseTablet();
                monitor.gameObject.SetActive(true);
                
                Logo.SetActive(true);
                yield break;
            }
            yield return null;
        }
        
    }

    private IEnumerator RotateTablet_co(float dir)
    {
        while(true)
        {
            rotTime += dir * Time.fixedDeltaTime*0.5f;
            if (rotTime<= 0f)
            {
                isTablet = false;
                render.enabled = false;
                monitor.gameObject.SetActive(false);
                monitorCanvas.gameObject.SetActive(false);
                isOn = false;
                rotTablet_co = null;
                yield break;
            }
            if(rotTime >= 1f)
            {
                isTablet = true;
                isOn = true;
                rotTablet_co = null;
                yield break;
            }
            transform.RotateAround(playerCam.transform.position, playerCam.transform.right, -dir*Time.fixedDeltaTime*enabledSpeed);
            
            
            yield return null;
        }
    }

    public void OnUseTrigger(int id)
    {
        if (id.Equals(clueTablet.ID))
        {
            if (isOn)
            {
                if (rotTablet_co != null)
                    StopCoroutine(rotTablet_co);

                rotTablet_co = StartCoroutine(RotateTablet_co(-1f));
            }
            else
            {
                if (rotTablet_co != null)
                    StopCoroutine(rotTablet_co);
                render.enabled = true;
                monitor.gameObject.SetActive(true);
                monitorCanvas.gameObject.SetActive(true);
                StartCoroutine(RotateTablet_co(1f));
            }
        }
    }
    
    public void SetUseTablet()
    {
        TriggerButton.OnUseTrigger += OnUseTrigger;
    }

    public void OnTouchStarted(Vector2 position)
    {
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
        if(DataSaveManager.Instance.GetGameState(floorIndex,spinIndex))
        {
            if (!isGet)
            {
                Ray ray = Camera.main.ScreenPointToRay(position);
                if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
                {
                    if (hit.collider.gameObject.Equals(gameObject))
                    {
                        clueTablet.GetItem(false);
                        StartCoroutine(SetTablet_co());
                        isGet = true;
                        col.enabled = false;
                        outline.enabled = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && outline != null)
        {
            outline.enabled = false;
        }
    }

    #region CutScene
    private void StartCutScene()
    {

    }
    #endregion
}

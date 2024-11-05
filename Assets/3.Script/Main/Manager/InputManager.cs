using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputData
{
    public InputAction action { get; private set; } // ��ġ�� ��ġ���� �߻� �� �׼�
    public Vector2 startValue { get; private set; } // ��ġ�� ���۵� ��ġ
    public Vector2 value { get; private set; } // ��ġ �� ���� ��ġ
    public bool isTouch { get; private set; } // ��ġ ������ Ȯ��
    public GameObject touchObject { get; private set; } //��ġ ���� ������Ʈ

    public InputData(InputAction action)
    {
        this.action = action;
        this.startValue = Vector2.zero;
        this.value = Vector2.zero;
        this.isTouch = false;
        this.touchObject = null;
    }

    
    public void SetStartTouchPos(Vector2 touchPos, GameObject touchObject = null)
    {
        startValue = touchPos;
        isTouch = true;
        this.touchObject = touchObject;
    }
    public void SetCurrentPos(Vector2 touchPos)
    {
        value = touchPos;
    }
    public void ResetData()
    {
        startValue = Vector2.zero;
        value = Vector2.zero;
        isTouch = false;
        touchObject = null;
    }
}

public class InputManager : MonoBehaviour
{
    private InputManager instance = null;
    public static InputManager Instance { get; private set; }

    private enum etouchState
    {
        Normal = 0,
        Player,
        UI,
        Object
    }
    [SerializeField] private LayerMask touchableObjectLayer; // ��ġ ������ ������Ʈ ���̾�
    
    [SerializeField] private InputActionAsset playerInput;

    [SerializeField] private Camera playerCamera;

    public Dictionary<int, InputData> activeActionDic { get; private set; } = new Dictionary<int, InputData>();

    private LayerMask systemUILayer;
    private LayerMask puzzleUILayer;

    private etouchState touchState;

    //������ Ŭ���� ����
    public InputData moveData { get; private set; }
    public InputData lookData { get; private set; }
    public InputData systemUIData{ get; private set; }
    public InputData puzzleUIData{ get; private set; }
    public InputData objectData { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        systemUILayer = LayerMask.GetMask("SystemUI");
        puzzleUILayer = LayerMask.GetMask("PuzzleUI");

       

        FindAction();
        
        AddEventPerformedAction();
        
        SetActionDisable(); // ��� �׼� Disable
        
        
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        foreach (int id in activeActionDic.Keys)
        {
            RomoveBindAction(id);
        }
        touchState = etouchState.Normal;
    }

    private void Update()
    {
        if (GameManager.Instance.isInput) return;
        foreach (var touch in Touchscreen.current.touches)
        {
            int touchId = touch.touchId.ReadValue() - 1;
            Vector2 touchPos = touch.position.ReadValue();

            if(touch.press.isPressed)
            {
                if (!activeActionDic.ContainsKey(touchId))
                {
                    if ((touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI))
                        && activeActionDic.Count < 3
                        && IsTouchOnUI(touchId))
                    {
                        // UI �׼� ����
                        GameObject UIObj = EventSystem.current.currentSelectedGameObject;
                        if(UIObj.layer == systemUILayer)
                        {
                            BindAction(touchId, systemUIData, touchPos, UIObj);
                        }
                        else if(UIObj.layer == puzzleUILayer)
                        {
                            BindAction(touchId, puzzleUIData, touchPos, UIObj);
                        }
                        touchState = etouchState.UI;

                    }
                    else if (IsTouchOnJoystickArea(touchPos)
                            && (touchState.Equals(etouchState.Normal)
                            || touchState.Equals(etouchState.Player)))
                    {
                        touchState = etouchState.Player;

                        BindAction(touchId, moveData, touchPos);


                    }
                    else if (touchState.Equals(etouchState.Normal) && IsTouchableObjectAtPosition(touchId, touchPos))
                    {
                        touchState = etouchState.Object;
                        
                    }
                    else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player))
                    {
                        if (IsTouchOnLeftScreen(touchPos) && !moveData.isTouch)
                        {
                            if (touchState.Equals(etouchState.Normal))
                                touchState = etouchState.Player;

                            BindAction(touchId, moveData, touchPos);
                        }
                        else if (IsTouchOnRightScreen(touchPos))
                        {
                            if (touchState.Equals(etouchState.Normal))
                                touchState = etouchState.Player;

                            BindAction(touchId, lookData, touchPos);
                        }
                    }
                }
                if(touchState.Equals(etouchState.Object)||touchState.Equals(etouchState.UI))
                {
                    if (activeActionDic.TryGetValue(touchId, out InputData value))
                    {
                        value.touchObject.TryGetComponent(out ReadInputData getData);
                        if (!getData.isTouch)
                        {
                            RomoveBindAction(touchId);
                        }
                    }
                }
                
            }
                
            

            if (!touch.press.isPressed && activeActionDic.ContainsKey(touchId))
            {
                RomoveBindAction(touchId);
            }

        }
        if (activeActionDic.Count.Equals(0))
        {
            touchState = etouchState.Normal;
        }
    }

    private bool IsTouchOnUI(int touchId)
    {
        // UI ��ġ �˻�
        
        return EventSystem.current.IsPointerOverGameObject(touchId+1);
    }

    private bool IsTouchOnJoystickArea(Vector2 touchPosition)
    {
        if (touchPosition.x < Screen.width / 4f &&touchPosition.y < Screen.height /2f)
        {
            return true;
        }
        return false;
    }

    // ��ġ�� ���� "Touchable Object"�� �ִ��� Ȯ���ϴ� �޼���
    private bool IsTouchableObjectAtPosition(int touchId, Vector2 touchPosition)
    {
        if (touchPosition.Equals(Vector2.zero)) return false;
        
        Ray ray = playerCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableObjectLayer))
        {
            // "Touchable Object" �±׸� ���� ������Ʈ�� �ִ��� Ȯ��
            if (hit.collider.CompareTag("touchableobject"))
            {
                BindAction(touchId, objectData, touchPosition, hit.collider.gameObject);
                return true;
            }
            if(hit.collider.CompareTag("EmptyLantern"))
            {
                GameManager.Instance.GetEmptyLantern();
                hit.collider.TryGetComponent(out ReadInputData lantern);
                lantern.isTouch = true;
                hit.collider.gameObject.SetActive(false);
            }

            if(hit.collider.CompareTag("Battery"))
            {
                GameManager.Instance.GetBattery();
                hit.collider.TryGetComponent(out ReadInputData lantern);
                lantern.isTouch = true;
                hit.collider.gameObject.SetActive(false);
            }
        }
        return false;
    }

    private bool IsTouchOnLeftScreen(Vector2 touchPosition)
    {
        // ȭ���� ���� ���� ��ġ �˻�
        return touchPosition.x < Screen.width / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnRightScreen(Vector2 touchPosition)
    {
        // ȭ���� ������ ���� ��ġ �˻�
        return touchPosition.x >= Screen.width / 2;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveData.SetCurrentPos(context.ReadValue<Vector2>());
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookData.SetCurrentPos(context.ReadValue<Vector2>());
    }
    private void OnSystemUI(InputAction.CallbackContext context)
    {
        systemUIData.SetCurrentPos(context.ReadValue<Vector2>());
    }
    private void OnPuzzleUI(InputAction.CallbackContext context)
    {
        puzzleUIData.SetCurrentPos(context.ReadValue<Vector2>());
    }
    private void OnObject(InputAction.CallbackContext context)
    {
        objectData.SetCurrentPos(context.ReadValue<Vector2>());
    }

    private void BindAction(int id, InputData data, Vector2 touchPos, GameObject Obj= null)
    {
        if(!activeActionDic.ContainsValue(data))
        {
            activeActionDic[id] = data;

            data.action.AddBinding($"<Touchscreen>/touch{id}/position");
            if (data == systemUIData||data == puzzleUIData||data == objectData)
            {
                data.SetStartTouchPos(touchPos,Obj);
            }
            else
            {
                data.SetStartTouchPos(touchPos);
            }
            
            data.action.Enable();
            if (Obj != null&&Obj.TryGetComponent(out ReadInputData setData))
            {
                setData.data = data;
                setData.Started();
            }
        }
        
    }
    private void RomoveBindAction(int id)
    {
        if(activeActionDic.TryGetValue(id, out var data))
        {
            activeActionDic.Remove(id);
            if(data==systemUIData||data == puzzleUIData||data ==objectData)
            {
                if (data.touchObject != null && data.touchObject.TryGetComponent(out ReadInputData setData))
                {
                    setData.data = null;
                    setData.Ended();
                }
            }
            data.action.Disable();
            data.action.RemoveAllBindingOverrides();

            data.ResetData();
        }
        
    }
    /// <summary>
    /// Action �ʱ�ȭ
    /// </summary>
    private void FindAction()
    {
        InputActionMap playerMap = playerInput.FindActionMap("Player");
        moveData = new InputData(playerMap.FindAction("Move"));
        lookData = new InputData(playerMap.FindAction("Look"));

        InputActionMap UIMap = playerInput.FindActionMap("UI");
        systemUIData = new InputData(UIMap.FindAction("SystemUI"));
        puzzleUIData = new InputData(UIMap.FindAction("PuzzleUI"));

        InputActionMap ObjMap = playerInput.FindActionMap("Interaction");
        objectData = new InputData(ObjMap.FindAction("Object"));
    }
    /// <summary>
    /// �� �������� Action�� �̺�Ʈ �߰�
    /// </summary>
    private void AddEventPerformedAction()
    {
        moveData.action.performed += OnMove;
        lookData.action.performed += OnLook;
        systemUIData.action.performed += OnSystemUI;
        puzzleUIData.action.performed += OnPuzzleUI;
        objectData.action.performed += OnObject;
    }
    /// <summary>
    /// �ʱ�ȭ �� �׼� ��Ȱ��ȭ
    /// </summary>
    private void SetActionDisable()
    {
        moveData.action.Disable();
        lookData.action.Disable();
        systemUIData.action.Disable();
        puzzleUIData.action.Disable();
        objectData.action.Disable();
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputData
{
    public InputAction action { get; private set; } // 터치된 위치에서 발생 할 액션
    public Vector2 startValue { get; private set; } // 터치가 시작된 위치
    public Vector2 value { get; private set; } // 터치 중 현재 위치
    public bool isTouch { get; private set; } // 터치 중인지 확인
    public GameObject touchObject { get; private set; } //터치 중인 오브젝트

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
    [SerializeField] private LayerMask touchableObjectLayer; // 터치 가능한 오브젝트 레이어
    
    [SerializeField] private InputActionAsset playerInput;

    [SerializeField] private Camera playerCamera;

    public Dictionary<int, InputData> activeActionDic { get; private set; } = new Dictionary<int, InputData>();

    private PointerEventData pointData;
    private List<RaycastResult> results = new List<RaycastResult>();
    private LayerMask systemUILayer;
    private LayerMask puzzleUILayer;

    private etouchState touchState;

    //데이터 클래스 정의
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
        
        SetActionDisable(); // 모든 액션 Disable
        
        
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
                        // UI 액션 실행
                        GameObject UIObj = EventSystem.current.currentSelectedGameObject;
                        if(UIObj.layer == systemUILayer)
                        {
                            Debug.Log("System");
                            BindAction(touchId, systemUIData, touchPos, UIObj);
                        }
                        else if(UIObj.layer == puzzleUILayer)
                        {
                            Debug.Log("Puzzle");
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
                        Debug.Log("Joystick");


                    }
                    else if (touchState.Equals(etouchState.Normal) && IsTouchableObjectAtPosition(touchId, touchPos))
                    {
                        touchState = etouchState.Object;
                        
                        Debug.Log("Object");
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
       
    }

    private bool IsTouchOnUI(int touchId)
    {
        // UI 터치 검사
        
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

    // 터치한 곳에 "Touchable Object"가 있는지 확인하는 메서드
    private bool IsTouchableObjectAtPosition(int touchId, Vector2 touchPosition)
    {
        if (touchPosition.Equals(Vector2.zero)) return false;
        Debug.Log("여기안옴?");
        Ray ray = playerCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableObjectLayer))
        {
            // "Touchable Object" 태그를 가진 오브젝트가 있는지 확인
            if (hit.collider.CompareTag("touchableobject"))
            {
                Debug.Log("여기안옴?");
                BindAction(touchId, objectData, touchPosition, hit.collider.gameObject);
                return true;
            }
        }
        return false;
    }

    private bool IsTouchOnLeftScreen(Vector2 touchPosition)
    {
        // 화면의 왼쪽 절반 터치 검사
        return touchPosition.x < Screen.width / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnRightScreen(Vector2 touchPosition)
    {
        // 화면의 오른쪽 절반 터치 검사
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
        if(activeActionDic.Count.Equals(0))
        {
            touchState = etouchState.Normal;
        }
    }
    /// <summary>
    /// Action 초기화
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
    /// 각 데이터의 Action에 이벤트 추가
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
    /// 초기화 후 액션 비활성화
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

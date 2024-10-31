using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputData
{
    public InputAction action { get; private set; } // 터치된 위치에서 발생 할 액션
    public Vector2 startValue { get; private set; } // 터치가 시작된 위치
    public Vector2 value { get; private set; } // 터치 중 현재 위치
    public bool isTouch { get; private set; } // 터치 중인지 확인

    public InputData(InputAction action)
    {
        this.action = action;
        this.startValue = Vector2.zero;
        this.value = Vector2.zero;
        this.isTouch = false;
    }

    
    public void SetStartTouchPos(int touchId, Vector2 touchPos)
    {
        startValue = touchPos;
        isTouch = true;
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
    
    [SerializeField] private RectTransform joystickArea; // Joystick Area
    [SerializeField] private LayerMask touchableUILayer; // 터치 가능한 UI 레이어
    [SerializeField] private LayerMask touchableObjectLayer; // 터치 가능한 오브젝트 레이어
    
    [SerializeField] private InputActionAsset playerInput;

    public Dictionary<int, InputData> activeActionDic { get; private set; }
    private GraphicRaycaster graphicRaycast;
    private etouchState touchState;
    //데이터 클래스 정의
    public InputData moveData { get; private set; }
    public InputData lookData { get; private set; }
    public InputData UI0Data{ get; private set; }
    public InputData UI1Data{ get; private set; }
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
        FindAction();
        
        AddEventPerformedAction();
        
        SetActionDisable(); // 모든 액션 Disable
        
        activeActionDic = new Dictionary<int, InputData>();
        touchState = etouchState.Normal;
    }
    
    private void Update()
    {
        //if (Touchscreen.current == null) return;
        Debug.Log(activeActionDic.Count);
        foreach (var touch in Touchscreen.current.touches)
        {
            int touchId = touch.touchId.ReadValue() - 1;
            Vector2 touchPos = touch.position.ReadValue();

            if(touch.press.isPressed && !activeActionDic.ContainsKey(touchId))
            {
                if (IsTouchOnUI(touchPos, touchId)
                    && touchId<2
                    && (touchState.Equals(etouchState.Normal)||touchState.Equals(etouchState.UI)))
                {
                    // UI 액션 실행
                    touchState = etouchState.UI;
                    
                    switch(touchId)
                    {
                        case 0:
                            BindAction(touchId, UI0Data, touchPos);
                            break;
                        case 1:
                            BindAction(touchId, UI1Data, touchPos);
                            break;
                    }
                }
                else if (IsTouchOnJoystickArea(touchPos)
                        && (touchState.Equals(etouchState.Normal) 
                        || touchState.Equals(etouchState.Player)))
                {
                    touchState = etouchState.Player;

                    BindAction(touchId, moveData, touchPos);


                }
                else if (IsTouchableObjectAtPosition(touchPos)&&touchState.Equals(etouchState.Normal))
                {
                    touchState = etouchState.Object;
                    BindAction(touchId, objectData, touchPos);
                }
                else if(touchState.Equals(etouchState.Normal)|| touchState.Equals(etouchState.Player))
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
            

            if (!touch.press.isPressed && activeActionDic.ContainsKey(touchId))
            {
                RomoveBindAction(touchId);
            }

        }
        
    }

    private bool IsTouchOnUI(Vector2 touchPosition, int touchId)
    {
        // UI 터치 검사


        return EventSystem.current.IsPointerOverGameObject(touchId);
    }

    private bool IsTouchOnJoystickArea(Vector2 touchPosition)
    {
        // JoystickArea 터치 검사
        return RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition);
    }

    // 터치한 곳에 "Touchable Object"가 있는지 확인하는 메서드
    private bool IsTouchableObjectAtPosition(Vector2 touchPosition)
    {
        if (touchPosition.Equals(Vector2.zero)) return false;

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableObjectLayer))
        {
            // "Touchable Object" 태그를 가진 오브젝트가 있는지 확인
            if (hit.collider.CompareTag("touchableobject"))
            {
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
    private void OnUI0(InputAction.CallbackContext context)
    {
        UI0Data.SetCurrentPos(context.ReadValue<Vector2>());
    }
    private void OnUI1(InputAction.CallbackContext context)
    {
        UI1Data.SetCurrentPos(context.ReadValue<Vector2>());
    }
    private void OnObject(InputAction.CallbackContext context)
    {
        objectData.SetCurrentPos(context.ReadValue<Vector2>());
    }

    private void BindAction(int id, InputData data, Vector2 touchPos)
    {
        if(!activeActionDic.ContainsValue(data))
        {
            activeActionDic[id] = data;
                data.action.AddBinding($"<Touchscreen>/touch{id}/position");
                data.SetStartTouchPos(id, touchPos);
            
            data.action.Enable();            
        }
        
    }
    private void RomoveBindAction(int id)
    {
        if(activeActionDic.TryGetValue(id, out var data))
        {
            activeActionDic.Remove(id);
            //resetAction[id] = false;
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
        UI0Data = new InputData(UIMap.FindAction("UI0"));
        UI1Data = new InputData(UIMap.FindAction("UI1"));

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
        UI0Data.action.performed += OnUI0;
        UI1Data.action.performed += OnUI1;
        objectData.action.performed += OnObject;
    }
    /// <summary>
    /// 초기화 후 액션 비활성화
    /// </summary>
    private void SetActionDisable()
    {
        moveData.action.Disable();
        lookData.action.Disable();
        UI0Data.action.Disable();
        UI1Data.action.Disable();
        objectData.action.Disable();
    }
}

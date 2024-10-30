using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private InputManager instance = null;
    public static InputManager Instance { get; private set; }

    [SerializeField] private RectTransform joystickArea; // Joystick Area
    [SerializeField] private LayerMask touchableLayer; // 터치 가능한 레이어
    private enum etouchState
    {
        Normal = 0,
        Player,
        UI,
        Object
    }
    private etouchState touchState;

    [SerializeField] private InputActionAsset playerInput;

    private Dictionary<int, InputAction> activeActionDic;

    #region Action 추가
    private InputAction moveAction;
    public Vector2 startMoveValue { get; private set; }
    public Vector2 moveValue { get; private set; }
    public bool isMove { get; private set; }

    private InputAction lookAction;
    public Vector2 startLookValue { get; private set; }
    public Vector2 lookValue { get; private set; }
    public bool isLook { get; private set; }

    private InputAction UI0Action;
    public Vector2 UI0TouchValue { get; private set; }
    public bool isUI0 { get; private set; }
    private InputAction UI1Action;
    public Vector2 UI1TouchValue { get; private set; }
    public bool isUI1 { get; private set; }
    private InputAction UI2Action;
    public Vector2 UI2TouchValue { get; private set; }
    public bool isUI2 { get; private set; }
    private InputAction UI3Action;
    public Vector2 UI3TouchValue { get; private set; }
    public bool isUI3 { get; private set; }

    private InputAction objectAction;
    public Vector2 objectTouchValue { get; private set; }
    public bool isObject { get; private set; }
    #endregion

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
        

        ActionDisable(); // 모든 액션 Disable
        
        activeActionDic = new Dictionary<int, InputAction>();
    }
    
    private void Update()
    {
        //if (Touchscreen.current == null) return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int touchId = touch.touchId.ReadValue() - 1;
            Vector2 touchPos = touch.position.ReadValue();

            if(touch.press.isPressed && !activeActionDic.ContainsKey(touchId))
            {
                
                if (IsTouchOnUI(touch.position.ReadValue(),touchId))
                {
                    // UI 액션 실행
                    touchState = etouchState.UI;
                    foreach(InputAction activeUIAction in activeActionDic.Values)
                    {
                        if(!activeActionDic.ContainsValue(activeUIAction))
                        {
                    //BindAction(touchId, activeUIAction);
                    //수정필요
                        }
                    }
                }
                else if (IsTouchOnJoystickArea(touchPos))
                {
                    touchState = etouchState.Player;

                    startMoveValue = touchPos;

                    BindAction(touchId, moveAction);

                    isMove = true;

                }
                else if (IsTouchableObjectAtPosition(touchPos))
                {
                    touchState = etouchState.Object;
                    Debug.Log("Good");
                }
                else
                {
                    if (IsTouchOnLeftScreen(touchPos) && !isMove)
                    {
                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;

                        startMoveValue = touchPos;
                        
                        BindAction(touchId, moveAction);

                        isMove = true;
                    }
                    else if (IsTouchOnRightScreen(touchPos))
                    {
                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;

                        BindAction(touchId, lookAction);

                        isLook = true;
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchableLayer))
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
        moveValue = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookValue = context.ReadValue<Vector2>();
    }
    private void OnUI0(InputAction.CallbackContext context)
    {
        UI0TouchValue = context.ReadValue<Vector2>();
    }
    private void OnUI1(InputAction.CallbackContext context)
    {
        UI1TouchValue = context.ReadValue<Vector2>();
    }
    private void OnUI2(InputAction.CallbackContext context)
    {
        UI2TouchValue = context.ReadValue<Vector2>();
    }
    private void OnUI3(InputAction.CallbackContext context)
    {
        UI3TouchValue = context.ReadValue<Vector2>();
    }
    private void OnObject(InputAction.CallbackContext context)
    {
        objectTouchValue = context.ReadValue<Vector2>();
    }

    private void BindAction(int id, InputAction action)
    {
        if(!activeActionDic.ContainsValue(action))
        {
            activeActionDic[id] = action;
            action.AddBinding($"<Touchscreen>/touch{id}/position");
            action.Enable();
        }
        
    }
    private void RomoveBindAction(int id)
    {
        if(activeActionDic.TryGetValue(id, out var action))
        {
            activeActionDic.Remove(id);
            //resetAction[id] = false;
            action.Disable();
            action.RemoveAllBindingOverrides();
            
            if (isMove)
            {
                startMoveValue = Vector2.zero;
                moveValue = Vector2.zero;
                isMove = false;
            }
            if (isLook)
            {
                lookValue = Vector2.zero;
                isLook = false;
            }
            
        }
        if(activeActionDic.Count.Equals(0))
        {
            touchState = etouchState.Normal;
        }
    }
    private void FindAction()
    {
        InputActionMap playerMap = playerInput.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        lookAction = playerMap.FindAction("Look");

        InputActionMap UIMap = playerInput.FindActionMap("UI");
        UI0Action = UIMap.FindAction("UI0");
        UI1Action = UIMap.FindAction("UI1");
        UI2Action = UIMap.FindAction("UI2");
        UI3Action = UIMap.FindAction("UI3");

        InputActionMap ObjMap = playerInput.FindActionMap("Interaction");
        objectAction = ObjMap.FindAction("Object");
    }
    private void AddEventPerformedAction()
    {
        moveAction.performed += OnMove;
        lookAction.performed += OnLook;
        UI0Action.performed += OnUI0;
        UI1Action.performed += OnUI1;
        UI2Action.performed += OnUI2;
        UI3Action.performed += OnUI3;
        objectAction.performed += OnObject;
    }
    private void ActionDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }
}

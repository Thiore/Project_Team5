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

        Player = 0,
        UI,
        Object
    }
    private etouchState touchState;

    [SerializeField] private InputActionAsset playerInput;

    private Dictionary<int, InputAction> activeActionDic;

    private InputAction moveAction;
    public Vector2 startMoveValue;
    public Vector2 moveValue;
    public bool isMove = false;

    private InputAction lookAction;
    public Vector2 startLookValue;
    public Vector2 lookValue;
    public bool isLook = false;

    //private bool isTouching = false;


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
    private void OnEnable()
    {
        // 모든 입력 이벤트가 발생할 때마다 OnTouchEvent를 호출
        InputSystem.onEvent += OnTouchEvent;
    }

    private void OnDisable()
    {
        // 스크립트가 비활성화될 때 이벤트 핸들러 해제
        InputSystem.onEvent -= OnTouchEvent;
    }
    private void Start()
    {
        InputActionMap playerMap = playerInput.FindActionMap("Player");
        moveAction = playerInput.FindAction("Move");
        lookAction = playerInput.FindAction("Look");

        InputActionMap UIMap = playerInput.FindActionMap("UI");

        InputActionMap ObjMap = playerInput.FindActionMap("Object");
       
        
        moveAction.performed += OnMove;

        lookAction.performed += OnLook;

        ActionDisable();
        
        activeActionDic = new Dictionary<int, InputAction>();
    }

    private void OnTouchEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if(device is Touchscreen touchscreen)
        {
            foreach(var touch in touchscreen.touches)
            {
                int touchId = touch.touchId.ReadValue();

                if(touch.phase.value == UnityEngine.InputSystem.TouchPhase.Began && !activeActionDic.ContainsKey(touchId))
                {
                    Vector2 touchPos = touch.position.ReadValue();
                    if (IsTouchOnUI(touch.position.ReadValue()))
                    {
                        // UI 액션 실행
                        touchState = etouchState.UI;

                    }
                    else if (IsTouchOnJoystickArea(touchPos))
                    {
                        touchState = etouchState.Player;
                        isMove = true;
                        startMoveValue = touchPos;
                        BindAction(touchId, moveAction);
                        
                    }
                    else if (IsTouchableObjectAtPosition(touchPos))
                    {
                        
                    }
                    else
                    {
                        
                    }

                    

                }
            }
        }
    }

    //private void Update()
    //{

    //    if (Touchscreen.current != null)
    //    {
    //        Debug.Log(1);
    //        foreach (var touch in Touchscreen.current.touches)
    //        {
    //            Debug.Log(touch.path);
              
    //            if (touch.isInProgress)
    //            {
    //                Vector2 touchPosition = touch.position.ReadValue();
    //                if (actionDic.Count.Equals(0))
    //                {
    //                    if (IsTouchOnUI(touchPosition))
    //                    {
    //                        // UI 액션 실행
    //                        isUI = true;

    //                    }
    //                    else if (IsTouchOnJoystickArea(touchPosition))
    //                    {
    //                        isPlayer = true;
    //                        //Debug.Log(Touchscreen.current.primaryTouch.touchId.ToString());
    //                        BindAction(Touchscreen.current.primaryTouch.touchId, moveAction);
    //                    }
    //                    else if (IsTouchableObjectAtPosition(touchPosition))
    //                    {
    //                        isObject = true;
    //                    }
    //                    else
    //                    {
    //                        isPlayer = true;
    //                    }
    //                }
                    
    //                if (!actionDic.TryGetValue(touch.touchId, out InputAction Value))
    //                {
    //                    if (IsTouchOnLeftScreen(touchPosition))
    //                    {
    //                        //Debug.Log($"OnMove called with touch position: {touchPosition}");
    //                        BindAction(touch.touchId, moveAction);
    //                        isMove = true;
    //                    }
    //                    else if (IsTouchOnRightScreen(touchPosition))
    //                    {
    //                        //Debug.Log($"OnMove called with touch position: {touchPosition}");
    //                        BindAction(touch.touchId, lookAction);
    //                        isLook = true;
    //                    }
    //                }
                    
                        
    //            }

    //        }
    //    }
        

    //}

    private bool IsTouchOnUI(Vector2 touchPosition)
    {
        // UI 터치 검사
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsTouchOnJoystickArea(Vector2 touchPosition)
    {
        // JoystickArea 터치 검사
        return RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition);
    }

    // 터치한 곳에 "Touchable Object"가 있는지 확인하는 메서드
    private bool IsTouchableObjectAtPosition(Vector2 touchPosition)
    {
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

    private bool IsTouchOnRightScreen(Vector2 touchPosition)
    {
        // 화면의 오른쪽 절반 터치 검사
        return touchPosition.x >= Screen.width / 2;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Touch 입력을 사용하여 값과 상태를 얻음
        Touch touch = context.ReadValue<Touch>();

        switch (context.phase)
        {
            case InputActionPhase.Started:
                
                startMoveValue = touch.position;
                moveValue = touch.position;
                break;
            case InputActionPhase.Performed:
                
                Debug.Log("performed");
                moveValue = touch.position;
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Canceled");
                startMoveValue = Vector2.zero;
                moveValue = Vector2.zero;
                //RomoveBindAction(moveAction);
                break;
        }
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        // Touch 입력을 사용하여 값과 상태를 얻음
        Touch touch = context.ReadValue<Touch>();

        switch (context.phase)
        {
            case InputActionPhase.Started:
                
                startLookValue = touch.position;
                lookValue = touch.position;
                break;
            case InputActionPhase.Performed:
                
                lookValue = touch.position;
                break;
            case InputActionPhase.Canceled:
                Debug.Log("lCanceled");
                startLookValue = Vector2.zero;
                lookValue = Vector2.zero;
                //RomoveBindAction(lookAction);
                break;
        }
    }

    private void BindAction(int id, InputAction action)
    {
        activeActionDic[id] = action;
        action.AddBinding($"<Touchscreen>/position");
        action.Enable();
    }
    private void RomoveBindAction(int id)
    {
        
        if(activeActionDic.TryGetValue(id, out var action))
        {
            action.Disable();
            activeActionDic.Remove(id); 
        }
    }

    private void ActionDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }
}

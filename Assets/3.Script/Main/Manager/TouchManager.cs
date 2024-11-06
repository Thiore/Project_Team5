using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public interface ITouchable
{
    public void OnTouchStarted(Vector2 position);
    public void OnTouchHold(Vector2 position);
    public void OnTouchEnd(Vector2 position);
}

public class TouchManager : MonoBehaviour
{
    private TouchManager instance = null;
    public static TouchManager Instance { get; private set; }
    private enum etouchState
    {
        Normal = 0,
        Player,
        UI,
        Object
    }

    private etouchState touchState;

    public event Action<Vector2> OnTouchStarted;
    public event Action<Vector2> OnTouchHold;
    public event Action<Vector2> OnTouchEnd;


    [SerializeField] private InputActionAsset inputAsset;
    private InputAction touchAction;
    private Dictionary<int, ITouchable> currentTouchDic; // 현재 터치된 오브젝트

    [SerializeField] private LayerMask touchableObjectLayer;

    private HashSet<int> activeTouchID = new HashSet<int>();// 활성화된 터치 ID 추적
    private bool isMove;
    private bool isLook;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Instance = instance;

            InputActionMap actionMap = inputAsset.FindActionMap("Input");
            touchAction = actionMap.FindAction("Touch");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnEnableTuchAction();
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnDisableTuchAction();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        touchState = etouchState.Normal;
        isMove = false;
        isLook = false;
    }

    private void Start()
    {
        

    }

    private void Update()
    {
        if (Touchscreen.current.touches.Count.Equals(0)) return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int touchId = touch.touchId.ReadValue();
            
            Vector2 position = touch.position.ReadValue();

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                if(IsTouchOnUI(touchId)&&(touchState.Equals(etouchState.Normal)|| touchState.Equals(etouchState.UI)))
                {
                    activeTouchID.Add(touchId);
                    //currentTouchDic.Add(touchId,)
                    touchState = etouchState.UI;
                }
                else if (IsTouchOnJoystickArea(position) &&
                         !isMove &&
                         (touchState.Equals(etouchState.Normal) ||
                         touchState.Equals(etouchState.Player)))
                {
                    activeTouchID.Add(touchId);
                    
                    if (touchState.Equals(etouchState.Normal))
                        touchState = etouchState.Player;

                }
                else if (IsTouchableObjectAtPosition(position) && 
                         (touchState.Equals(etouchState.Normal) || 
                         touchState.Equals(etouchState.Object)))
                {
                    activeTouchID.Add(touchId);

                    touchState = etouchState.Object;

                }
                else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player))
                {
                    if (IsTouchOnLeftScreen(position) && !isMove)
                    {
                        activeTouchID.Add(touchId);

                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;

                        
                    }
                    else if (IsTouchOnRightScreen(position) && !isLook)
                    {
                        activeTouchID.Add(touchId);

                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;

                    }
                }

            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                //currentTouchDic
                //currentTouchObj?.OnTouchStarted(position); // 터치가 이동할 때만 현재 오브젝트에 이벤트 전달
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                //HandleTouchEnd(position);
            }
        }

    }
    private bool IsTouchOnUI(int touchId)
    {
        return EventSystem.current.IsPointerOverGameObject(touchId);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnJoystickArea(Vector2 touchPosition)
    {
        if (touchPosition.x < Screen.width / 4f && touchPosition.y < Screen.height / 2f)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchableObjectAtPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, touchableObjectLayer))
        {
            if (hit.collider.CompareTag("touchableobject"))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnLeftScreen(Vector2 touchPosition)
    {
        return touchPosition.x < Screen.width / 2;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnRightScreen(Vector2 touchPosition)
    {
        return touchPosition.x >= Screen.width / 2;
    }
    public void OnEnableTuchAction()
    {
        touchAction.Enable();
    }
    public void OnDisableTuchAction()
    {
        touchAction.Disable();
    }

    public void TouchStart()
    {

    }
}

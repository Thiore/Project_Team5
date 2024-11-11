using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public interface ITouchable
{
    /// <summary>
    /// 터치 시작 시 호출
    /// </summary>
    /// <param name="touchPosition">화면에 터치가 시작된 좌표</param>
    public void OnTouchStarted(Vector2 position);
    /// <summary>
    /// 터치 이동 시 호출
    /// </summary>
    /// <param name="touchPosition">현재 터치한 위치의 좌표</param>
    public void OnTouchHold(Vector2 position);
    /// <summary>
    /// 터치 종료시 호출 및 초기화
    /// </summary>
    /// <param name="touchPosition">터치가 종료되는 위치의 좌표</param>
    public void OnTouchEnd(Vector2 position);
}


public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance { get; private set; } = null;

    private enum etouchState
    {
        Normal = 0,
        Player,
        UI,
        Object
    }
    

    private etouchState touchState;

    public event Action<Vector2> OnMoveStarted;
    public event Action<Vector2> OnMoveHold;
    public event Action<Vector2> OnMoveEnd;

    public event Action<Vector2> OnLookStarted;
    public event Action<Vector2> OnLookHold;
    public event Action<Vector2> OnLookEnd;


    [SerializeField] private InputActionAsset inputAsset;
    private InputAction touchAction;
    private Dictionary<int, ITouchable> currentTouchDic; // 현재 터치된 오브젝트

    [SerializeField] private LayerMask touchableObjectLayer;
    private LayerMask dontTouchableObjectLayer;

    private HashSet<int> activeTouchID;// 활성화된 터치 ID 추적
    private int moveID;
    private int lookID;

    [SerializeReference] private float touchDistance;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("touch");
            Instance = this;

            InputActionMap actionMap = inputAsset.FindActionMap("Input");
            touchAction = actionMap.FindAction("Touch");

            dontTouchableObjectLayer = ~touchableObjectLayer;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
           // Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnEnableTuchAction();
    }
    private void OnDisable()
    {
        OnDisableTuchAction();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentTouchDic = new Dictionary<int, ITouchable>();
        activeTouchID = new HashSet<int>();
        touchState = etouchState.Normal;
        moveID = -1;
        lookID = -1;
    }

    //private void Start()
    //{
    //    if(touchDistance>100f||touchDistance<1f)
    //    {
    //        touchDistance = 5f;
    //    }

    //}

    private void Update()
    {
        if (Touchscreen.current.touches.Count.Equals(0)) return;

        foreach (var touch in Touchscreen.current.touches)
        {
            int touchId = touch.touchId.ReadValue();
            
            
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Vector2 position = touch.position.ReadValue();
                if (IsTouchOnUI(touchId) &&
                    (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI)))
                {
                    activeTouchID.Add(touchId);

                    touchState = etouchState.UI;
                }
                else if (IsTouchOnJoystickArea(position) &&
                         moveID.Equals(-1) &&
                         (touchState.Equals(etouchState.Normal) ||
                         touchState.Equals(etouchState.Player)))
                {
                    activeTouchID.Add(touchId);
                    moveID = touchId;
                    OnMoveStarted?.Invoke(position);

                    if (touchState.Equals(etouchState.Normal))
                        touchState = etouchState.Player;

                }       
                else if (IsTouchableObjectAtPosition(touchId, position))
                {
                    activeTouchID.Add(touchId);

                    touchState = etouchState.Object;

                }
                else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player))
                {
                    if (IsTouchOnLeftScreen(position) && moveID.Equals(-1))
                    {
                        activeTouchID.Add(touchId);
                        moveID = touchId;
                        OnMoveStarted?.Invoke(position);

                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;
                    }
                    else if (IsTouchOnRightScreen(position) && lookID.Equals(-1))
                    {
                        activeTouchID.Add(touchId);
                        lookID = touchId;
                        OnLookStarted?.Invoke(position);

                        if (touchState.Equals(etouchState.Normal))
                            touchState = etouchState.Player;

                    }
                }

            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                if (touchState.Equals(etouchState.UI)) return;

                    Vector2 position = touch.position.ReadValue();
                    switch (touchState)
                    {
                        case etouchState.Player:
                            if (moveID.Equals(touchId))
                            {
                                OnMoveHold?.Invoke(position);
                            }
                            if (lookID.Equals(touchId))
                            {
                                OnLookHold?.Invoke(position);
                            }
                            break;
                        case etouchState.Object:
                            currentTouchDic[touchId]?.OnTouchHold(position);
                            break;
                    }
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {

                
                    Vector2 position = touch.position.ReadValue();
                    switch (touchState)
                    {
                        case etouchState.Player:
                            if (moveID.Equals(touchId))
                            {
                                OnMoveEnd?.Invoke(position);
                                
                                moveID = -1;
                            }
                            if (lookID.Equals(touchId))
                            {
                                
                                OnLookEnd?.Invoke(position);
                                lookID = -1;
                            }
                            if (moveID.Equals(-1) && lookID.Equals(-1))
                            {
                                touchState = etouchState.Normal;
                            }
                            break;
                        case etouchState.UI:
                            
                            if (activeTouchID.Count.Equals(0))
                            {
                                touchState = etouchState.Normal;
                            }

                            break;
                        case etouchState.Object:
                            currentTouchDic[touchId]?.OnTouchEnd(position);
                            currentTouchDic.Remove(touchId);
                            
                            if (currentTouchDic.Count.Equals(0))
                            {
                                touchState = etouchState.Normal;
                            }
                            break;
                    }
                activeTouchID.Remove(touchId);

            }
        }
    }

    //private void OnTouchStarted(InputAction.CallbackContext context)
    //{
    //    var touchControl = context.control;
    //    if (touchControl == null) return;

    //    int a = touchControl.

    //    foreach (var touch in Touchscreen.current.touches)
    //    {

    //        if (!context.Equals(touch))
    //        {
    //            Debug.Log(context);
    //            Debug.Log(touch);
    //            continue;
    //        }
    //        int touchId = touch.touchId.ReadValue();
    //        if (activeTouchID.Contains(touchId)) continue;
    //        Vector2 position = touch.position.ReadValue();


    //        if (IsTouchOnUI(touchId) &&
    //            (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI)))
    //        {
    //            activeTouchID.Add(touchId);

    //            touchState = etouchState.UI;
    //        }
    //        else if (IsTouchOnJoystickArea(position) &&
    //                 moveID.Equals(-1) &&
    //                 (touchState.Equals(etouchState.Normal) ||
    //                 touchState.Equals(etouchState.Player)))
    //        {
    //            activeTouchID.Add(touchId);
    //            moveID = touchId;
    //            OnMoveStarted?.Invoke(position);

    //            if (touchState.Equals(etouchState.Normal))
    //                touchState = etouchState.Player;

    //        }
    //        else if (IsTouchableObjectAtPosition(touchId, position) &&
    //                 (touchState.Equals(etouchState.Normal) ||
    //                 touchState.Equals(etouchState.Object)))
    //        {
    //            activeTouchID.Add(touchId);

    //            touchState = etouchState.Object;

    //        }
    //        else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player))
    //        {
    //            if (IsTouchOnLeftScreen(position) && moveID.Equals(-1))
    //            {
    //                activeTouchID.Add(touchId);
    //                moveID = touchId;
    //                OnMoveStarted?.Invoke(position);

    //                if (touchState.Equals(etouchState.Normal))
    //                    touchState = etouchState.Player;
    //            }
    //            else if (IsTouchOnRightScreen(position) && lookID.Equals(-1))
    //            {
    //                activeTouchID.Add(touchId);
    //                lookID = touchId;
    //                OnLookStarted?.Invoke(position);

    //                if (touchState.Equals(etouchState.Normal))
    //                    touchState = etouchState.Player;

    //            }
    //        }


    //    }
    //}
    //private void OnTouchPerformed(InputAction.CallbackContext context)
    //{

    //    if (touchState.Equals(etouchState.UI)) return;
    //    foreach (var touch in Touchscreen.current.touches)
    //    {
    //        if (!context.Equals(touch))
    //        {
    //            Debug.Log(context);
    //            Debug.Log(touch);
    //            continue;
    //        }


    //        int touchId = touch.touchId.ReadValue();
    //        if (!activeTouchID.Contains(touchId)) continue;

    //        Vector2 position = touch.position.ReadValue();

    //        if (activeTouchID.Contains(touchId))
    //        {
    //            switch (touchState)
    //            {
    //                case etouchState.Player:
    //                    if (moveID.Equals(touchId))
    //                    {
    //                        OnMoveHold?.Invoke(position);
    //                    }
    //                    if (lookID.Equals(touchId))
    //                    {
    //                        OnLookHold?.Invoke(position);
    //                    }
    //                    break;
    //                case etouchState.Object:
    //                    currentTouchDic[touchId].OnTouchHold(position);
    //                    break;
    //            }
    //        }
    //    }
    //    }

    //private void OnTouchCanceled(InputAction.CallbackContext context)
    //{
    //    foreach (var touch in Touchscreen.current.touches)
    //    {
    //        if (!context.Equals(touch))
    //        {
    //            Debug.Log(context);
    //            Debug.Log(touch);
    //            continue;
    //        }
    //        int touchId = touch.touchId.ReadValue();
    //        Vector2 position = touch.position.ReadValue();
    //        if (activeTouchID.Contains(touchId))
    //        {
    //            switch (touchState)
    //            {
    //                case etouchState.Player:
    //                    if (moveID.Equals(touchId))
    //                    {
    //                        OnMoveEnd?.Invoke(position);
    //                        activeTouchID.Remove(touchId);
    //                        moveID = -1;
    //                    }
    //                    if (lookID.Equals(touchId))
    //                    {
    //                        activeTouchID.Remove(touchId);
    //                        OnLookEnd?.Invoke(position);
    //                        lookID = -1;
    //                    }
    //                    if (moveID.Equals(-1) && lookID.Equals(-1))
    //                    {
    //                        touchState = etouchState.Normal;
    //                    }
    //                    break;
    //                case etouchState.UI:
    //                    activeTouchID.Remove(touchId);
    //                    if (activeTouchID.Count.Equals(0))
    //                    {
    //                        touchState = etouchState.Normal;
    //                    }

    //                    break;
    //                case etouchState.Object:
    //                    currentTouchDic[touchId].OnTouchEnd(position);
    //                    currentTouchDic.Remove(touchId);
    //                    activeTouchID.Remove(touchId);
    //                    if (currentTouchDic.Count.Equals(0))
    //                    {
    //                        touchState = etouchState.Normal;
    //                    }
    //                    break;
    //            }
    //        }

    //    }
    //    }

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
    /// 터치한 오브젝트가 터치가능한 오브젝트인지 검사합니다.
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchableObjectAtPosition(int touchId, Vector2 touchPosition)
    {

        if(touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Object))
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, touchDistance, dontTouchableObjectLayer))
            {
                return false;
            }
            else
            {
                if (hit.collider.TryGetComponent(out ITouchable touchable))
                {
                    if (currentTouchDic.ContainsValue(touchable))
                        return false;
                    currentTouchDic.Add(touchId, touchable);
                    currentTouchDic[touchId].OnTouchStarted(touchPosition);
                    Debug.Log("이거");
                    return true;
                }

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
        //touchAction.started += ctx => OnTouchStarted(ctx);
        //touchAction.performed += ctx => OnTouchPerformed(ctx);
        //touchAction.canceled += ctx => OnTouchCanceled(ctx);
    }
    public void OnDisableTuchAction()
    {
        touchAction.Disable();
        //touchAction.started -= ctx => OnTouchStarted(ctx);
        //touchAction.performed -= ctx => OnTouchPerformed(ctx);
        //touchAction.canceled -= ctx => OnTouchCanceled(ctx);
    }

    
    
}
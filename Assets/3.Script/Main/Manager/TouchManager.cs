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
    public LayerMask getTouchableLayer { get; private set; }
    [SerializeField] private LayerMask playerLayer;

    private HashSet<int> activeTouchID;// 활성화된 터치 ID 추적
    private int moveID;
    private int lookID;

    [SerializeReference] private float touchDistance;
    public float getTouchDistance { get; private set; }

    private bool isMoving;

    private void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("touch");
            Instance = this;

            InputActionMap actionMap = inputAsset.FindActionMap("Input");
            touchAction = actionMap.FindAction("Touch");

            getTouchableLayer = touchableObjectLayer;
            getTouchDistance = touchDistance;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
           // Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        currentTouchDic = new Dictionary<int, ITouchable>();
        activeTouchID = new HashSet<int>();
        touchState = etouchState.Normal;
        moveID = -1;
        lookID = -1;
        OnEnableTuchAction();
        isMoving = true;
        //SceneManager.sceneLoaded += OnTouchLoaded;
        
    }
    //private void OnDisable()
    //{
    //    OnDisableTuchAction();
    //    SceneManager.sceneLoaded -= OnTouchLoaded;
        
    //}

    //private void OnTouchLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    currentTouchDic = new Dictionary<int, ITouchable>();
    //    activeTouchID = new HashSet<int>();
    //    touchState = etouchState.Normal;
    //    moveID = -1;
    //    lookID = -1;
    //    OnEnableTuchAction();
    //    isMoving = true;
    //}

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
           
            
            
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                int touchId = touch.touchId.ReadValue();
                Vector2 position = touch.position.ReadValue();
                if (IsTouchOnUI(touchId) &&
                    (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI)))
                {
                    activeTouchID.Add(touchId);

                    touchState = etouchState.UI;
                }
                else if (isMoving &&
                         IsTouchOnJoystickArea(position) &&
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
                else if (isMoving&&
                        (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player)))
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
                int touchId = touch.touchId.ReadValue();
                if (touchState.Equals(etouchState.UI)) return;

                Vector2 position = touch.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(position);
                Debug.DrawRay(ray.origin, ray.direction * touchDistance, Color.red);
                switch (touchState)
                {
                    case etouchState.Player:
                        if (isMoving)
                        {
                            if (moveID.Equals(touchId))
                            {
                                OnMoveHold?.Invoke(position);
                            }
                            if (lookID.Equals(touchId))
                            {
                                OnLookHold?.Invoke(position);
                            }
                        }
                        else
                        {
                            if (moveID.Equals(touchId))
                            {
                                OnMoveEnd?.Invoke(position);
                            }
                            if (lookID.Equals(touchId))
                            {
                                OnLookEnd?.Invoke(position);
                            }
                            touchState = etouchState.Normal;
                        }

                        break;
                    case etouchState.Object:
                        if (currentTouchDic.ContainsKey(touchId))
                        {
                            currentTouchDic[touchId]?.OnTouchHold(position);
                        }

                        break;
                }
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                int touchId = touch.touchId.ReadValue();

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

                    case etouchState.Object:

                        if (currentTouchDic.ContainsKey(touchId))
                        {
                            Debug.Log("제발"+touchId);
                            currentTouchDic[touchId]?.OnTouchEnd(position);
                            Debug.Log("안뺌"+touchId);
                            currentTouchDic.Remove(touchId);
                            Debug.Log("뺌" + touchId);
                        }
                        if (currentTouchDic.Count == 0)
                        {
                            touchState = etouchState.Normal;
                        }
                        break;
                    default:
                        break;
                }
                if(activeTouchID != null)
                {
                    if (activeTouchID.Contains(touchId))
                        activeTouchID.Remove(touchId);

                    if (activeTouchID.Count.Equals(0))
                    {
                        touchState = etouchState.Normal;
                        currentTouchDic.Clear();
                    }
                }
                    

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
            //레이케스트가 터치가능 오브젝트에 충돌했거나 어디에도 충돌되지 않았을 때
            if (Physics.Raycast(ray, out hit, touchDistance))
            {
                if (hit.collider == null) return false;

                int hitLayer = hit.collider.gameObject.layer;

                // `touchableObjectLayer`에 hit 객체의 레이어가 포함되어 있는지 확인
                bool isTouchableObject = (touchableObjectLayer.value & (1 << hitLayer)) != 0;

                if(isTouchableObject)
                {
                    if (hit.collider.TryGetComponent(out ITouchable touchable))
                    {
                        if (currentTouchDic.ContainsValue(touchable))
                            return false;
                        currentTouchDic.Add(touchId, touchable);
                        currentTouchDic[touchId].OnTouchStarted(touchPosition);
                        Debug.Log(touchable);
                        return true;
                    }
                }     

                return false;
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


    
    public void EnableMoveHandler(bool dontTouch)
    {
        isMoving = dontTouch;
    }
}
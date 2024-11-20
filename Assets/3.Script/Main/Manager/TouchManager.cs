using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
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

public class TouchObj
{
    public ITouchable touchable;
    public Vector2 position;

    public TouchObj(ITouchable Touchable, Vector2 Position)
    {
        touchable = Touchable;
        position = Position;
    }
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

    private EventSystem eventSystem;

    
    private Dictionary<int, TouchObj> currentTouchDic; // 현재 터치된 오브젝트

    [SerializeField] private LayerMask touchableObjectLayer;
    public LayerMask getTouchableLayer { get => touchableObjectLayer; }
    [SerializeField] private LayerMask playerLayer;

    private HashSet<int> UIID;// 활성화된 터치 ID 추적
    private HashSet<ITouchable> activeTouchObj;// 활성화중인 오브젝트를 검사하기위해 담아둠
    

    private int moveId;
    private int lookId;

    [SerializeReference] private float touchDistance;
    public float getTouchDistance { get => touchDistance; }

    private bool isMoving; // 미니게임 등 이동을 막아야할때 사용
    private bool isTouching; // 컷신등 터치를 완전히 막아야할때 사용

    private void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("touch");
            Instance = this;

            //InputActionMap actionMap = inputAsset.FindActionMap("Input");
            //touchAction = actionMap.FindAction("Touch");
            currentTouchDic = new Dictionary<int, TouchObj>();
            UIID = new HashSet<int>();
            activeTouchObj = new HashSet<ITouchable>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
           Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnTouchLoaded;
    }
    private void OnDisable()
    {
        OnDisableTuchAction();
        SceneManager.sceneLoaded -= OnTouchLoaded;

    }

    private void OnTouchLoaded(Scene scene, LoadSceneMode mode)
    {
        OnEnableTuchAction();
    }


    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Started");
    
            Vector2 position = Vector2.zero;
        int touchId = -1;
            if ((touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI)) && IsTouchOnUI(touchId))
            {
                UIID.Add(touchId);

                touchState = etouchState.UI;
            }
            else if (IsTouchOnJoystickArea(position) &&
                     moveId.Equals(-1) &&
                     (touchState.Equals(etouchState.Normal) ||
                     touchState.Equals(etouchState.Player)))
            {

                moveId = touchId;


                if (touchState.Equals(etouchState.Normal))
                {
                    touchState = etouchState.Player;
                    if (eventSystem.enabled)
                    {
                        eventSystem.enabled = false;
                    }
                }
                OnMoveStarted?.Invoke(position);
            }
            else if ((touchState.Equals(etouchState.Normal) ||
                     touchState.Equals(etouchState.Object)) &&
                     IsTouchableObjectAtPosition(touchId, position))
            {

                currentTouchDic[touchId].touchable.OnTouchStarted(position);
                currentTouchDic[touchId].position = position;
            }
            else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player))
            {
                if (IsTouchOnLeftScreen(position) && moveId.Equals(-1))
                {

                    moveId = touchId;


                    if (touchState.Equals(etouchState.Normal))
                    {
                        touchState = etouchState.Player;
                        if (eventSystem.enabled)
                        {
                            eventSystem.enabled = false;
                        }
                    }
                    OnMoveStarted?.Invoke(position);
                }
                else if (IsTouchOnRightScreen(position) && lookId.Equals(-1))
                {

                    lookId = touchId;

                    if (touchState.Equals(etouchState.Normal))
                    {
                        touchState = etouchState.Player;
                        if (eventSystem.enabled)
                        {
                            eventSystem.enabled = false;
                        }
                    }
                    OnLookStarted?.Invoke(position);

                }
            }
        

    }
    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Performed");
        if (touchState.Equals(etouchState.UI)) return;
      

        Vector2 position = Vector2.zero;
            int touchId = -1;
            switch (touchState)
            {
                case etouchState.Player:
                    if (moveId.Equals(touchId))
                    {
                        OnMoveHold?.Invoke(position);
                    }
                    if (lookId.Equals(touchId))
                    {
                        OnLookHold?.Invoke(position);
                    }
                    break;
                case etouchState.Object:
                    currentTouchDic[touchId].touchable?.OnTouchHold(position);
                    currentTouchDic[touchId].position = position;
                    break;
            }
        
        
        
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Canceled");
        
            Vector2 position = Vector2.zero;
            int touchId = -1;
            switch (touchState)
            {
                case etouchState.Player:
                    if (moveId.Equals(touchId))
                    {
                        OnMoveEnd?.Invoke(position);

                        moveId = -1;
                    }
                    if (lookId.Equals(touchId))
                    {
                        OnLookEnd?.Invoke(position);
                        lookId = -1;
                    }
                    if (moveId.Equals(-1) && lookId.Equals(-1))
                    {
                        touchState = etouchState.Normal;
                        eventSystem.enabled = true;
                    }
                    break;
                case etouchState.UI:
                    UIID.Remove(touchId);
                    if (UIID.Count.Equals(0))
                    {
                        touchState = etouchState.Normal;
                    }

                    break;
                case etouchState.Object:
                    currentTouchDic[touchId].touchable?.OnTouchEnd(position);
                    activeTouchObj.Remove(currentTouchDic[touchId].touchable);
                    currentTouchDic.Remove(touchId);
                    if (currentTouchDic.Count.Equals(0))
                    {
                        touchState = etouchState.Normal;
                        eventSystem.enabled = true;

                    }
                    break;
            
        }
    }

    private bool IsTouchOnUI(int touchId)
    {
        if (EventSystem.current.IsPointerOverGameObject(touchId))
        {
            Debug.Log("UI 터치 감지 성공");
            Debug.Log($"Touch ID: {touchId}, UI Hit: {EventSystem.current.IsPointerOverGameObject(touchId)}");
        }
        else
        {
            Debug.Log("UI 외의 터치");
            Debug.Log($"Touch ID: {touchId}, UI Hit: {EventSystem.current.IsPointerOverGameObject(touchId)}");
        }
        return EventSystem.current.IsPointerOverGameObject(touchId);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchOnJoystickArea(Vector2 touchPosition)
    {
        if (touchPosition.x < Screen.width / 5f && touchPosition.y < Screen.height / 3f)
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
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;
        //레이케스트가 터치가능 오브젝트에 충돌했거나 어디에도 충돌되지 않았을 때
        if (Physics.Raycast(ray, out hit, touchDistance))
        {
            if (hit.collider == null) return false;

            int hitLayer = hit.collider.gameObject.layer;

            // `touchableObjectLayer`에 hit 객체의 레이어가 포함되어 있는지 확인
            bool isTouchableObject = (touchableObjectLayer.value & (1 << hitLayer)) != 0;

            if (isTouchableObject)
            {
                if (hit.collider.TryGetComponent(out ITouchable touchable))
                {
                    
                    if (activeTouchObj.Contains(touchable)) return false;

                    if (eventSystem.enabled)
                    {
                        eventSystem.enabled = false;
                    }

                    currentTouchDic.Add(touchId, new TouchObj(touchable,touchPosition));
                    activeTouchObj.Add(touchable);

                    if (touchState.Equals(etouchState.Object))
                        touchState = etouchState.Object;

                    return true;
                }
            }
            return false;
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
        Debug.Log("추가됨");
            touchState = etouchState.Normal;
            if(eventSystem != null)
            {
                eventSystem.enabled = true;
            }
            else
            {
                eventSystem = FindObjectOfType<EventSystem>();
                eventSystem.enabled = true;
                Debug.Log("이벤트시스템 없음");
            }
            
            moveId = -1;
            lookId = -1;
            isMoving = true;
            currentTouchDic.Clear();
            UIID.Clear();
        activeTouchObj.Clear();

        EnhancedTouchSupport.Enable();

        //Touch.onFingerDown += OnTouchStarted;
        //Touch.onFingerMove.performed += OnTouchPerformed;
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnTouchCanceled;
        
    }
    public void OnDisableTuchAction()
    {
        
        //pressAction.Disable();
        //positionAction.Disable();
        //pressAction.started -=OnTouchStarted;
        //positionAction.performed -= OnTouchPerformed;
        //pressAction.canceled -= OnTouchCanceled;

            switch (touchState)
            {
                case etouchState.Normal:
                    break;
                case etouchState.Player:
                    touchState = etouchState.Normal;
                    if (!moveId.Equals(-1))
                    {
                        OnMoveEnd?.Invoke(Vector2.zero);
                        moveId = -1;
                    }
                    if (!lookId.Equals(-1))
                    {
                        OnLookEnd?.Invoke(Vector2.zero);
                        lookId = -1;
                    }
                    eventSystem.enabled = true;
                    break;
                case etouchState.UI:
                    touchState = etouchState.Normal;
                    break;
                case etouchState.Object:
                    touchState = etouchState.Normal;
                    if (!currentTouchDic.Count.Equals(0))
                    {
                        foreach (int touchId in currentTouchDic.Keys)
                        {
                            currentTouchDic[touchId].touchable?.OnTouchEnd(currentTouchDic[touchId].position);
                            activeTouchObj.Remove(currentTouchDic[touchId].touchable);
                            currentTouchDic.Remove(touchId);
                        }
                        eventSystem.enabled = true;
                    }
                    break;
            }

            currentTouchDic.Clear();
            activeTouchObj.Clear();
            UIID.Clear();
        
    }

    public void EnableMoveHandler(bool dontTouch)
    {
        isMoving = dontTouch;
        if(dontTouch.Equals(false))
        {
            if(touchState.Equals(etouchState.Player))
            {
                eventSystem.enabled = true;
                touchState = etouchState.Normal;
                if (!moveId.Equals(-1))
                {
                    OnMoveEnd?.Invoke(Vector2.zero);
                    moveId = -1;
                }
                if (!lookId.Equals(-1))
                {
                    OnLookEnd?.Invoke(Vector2.zero);
                    lookId = -1;
                }
            }
        }
    }

    private int GetTouchId(Vector2 position)
    {
        foreach(var touch in Touchscreen.current.touches)
        {
            if(touch.position.ReadValue().Equals(position))
            {
                return touch.touchId.ReadValue();
            }
        }
        return -1;
    }
}

//private void Start()
//{
//    if(touchDistance>100f||touchDistance<1f)
//    {
//        touchDistance = 5f;
//    }

//}
/*
private void Update()
{
    if(isTouching)
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
                else if (isMoving &&
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
                            Debug.Log("제발" + touchId);
                            currentTouchDic[touchId]?.OnTouchEnd(position);
                            Debug.Log("안뺌" + touchId);
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
*/
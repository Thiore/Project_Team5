using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
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
public interface IUITouchable
{
    public void OnUIStarted(PointerEventData data);
    public void OnUIHold(PointerEventData data);
    public void OnUIEnd(PointerEventData data);


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
    

    private Dictionary<int, ITouchable> currentTouchDic; // 현재 터치된 오브젝트
    private Dictionary<int, IUITouchable> currentUIDic; // 현재 터치된 오브젝트

    [SerializeField] private LayerMask touchableObjectLayer;
    public LayerMask getTouchableLayer { get => touchableObjectLayer; }
    [SerializeField] private LayerMask ignoreLayer;

    private HashSet<int> UIID;// 활성화된 터치 ID 추적


    private int moveId;
    private int lookId;

    [SerializeReference] private float touchDistance;
    public float getTouchDistance { get => touchDistance; }

    private bool isMoving; // 미니게임 등 이동을 막아야할때 사용

    private bool isTouching;

    private bool isTouchSupportEnabled; // EnhancedTouch 상태 관리

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isTouchSupportEnabled = false;
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
        
        SceneManager.sceneLoaded -= OnTouchLoaded;

    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("sk");
            OnDisableTouchAction();
        }
        else
            OnEnableTouchAction();
    }

    private void OnApplicationQuit()
    {
        OnDisableTouchAction();
    }

    private void OnTouchLoaded(Scene scene, LoadSceneMode mode)
    {
        eventSystem = EventSystem.current;
        OnDisableTouchAction();
        StartCoroutine(EnableEventSystem());


        OnEnableTouchAction();
    }


    private void OnTouchStarted(Finger finger)
    {

        Vector2 position = finger.screenPosition;
        int touchId = finger.index;

        if (IsTouchOnUI(touchId,position))
        {
            UIID.Add(touchId);
            

            touchState = etouchState.UI;
        }
        else if(isTouching)
        {
            if (moveId.Equals(-1) &&
                 isMoving &&
                 (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player)) &&
                 IsTouchOnJoystickArea(position))
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
            else if ((touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Object)) &&
                     IsTouchableObjectAtPosition(touchId, position))
            {
                if (eventSystem.enabled)
                {
                    eventSystem.enabled = false;
                }

                currentTouchDic[touchId].OnTouchStarted(position);

                if (touchState.Equals(etouchState.Normal))
                    touchState = etouchState.Object;
            }
            else if (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.Player) && isMoving)
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
    }
    private void OnTouchPerformed(Finger finger)
    {
        Vector2 position = finger.screenPosition;
        int touchId = finger.index;
        if (touchState.Equals(etouchState.UI))
        {
            if (currentUIDic.TryGetValue(touchId, out IUITouchable touch))
            {
                PointerEventData pointerData = new PointerEventData(eventSystem)
                {
                    position = position
                };
                touch?.OnUIHold(pointerData);
            }

        }
        if(isTouching)
        {
            
            switch (touchState)
            {
                case etouchState.Player:
                    if (isMoving)
                    {
                        if (moveId.Equals(touchId))
                        {
                            OnMoveHold?.Invoke(position);
                        }
                        if (lookId.Equals(touchId))
                        {
                            OnLookHold?.Invoke(position);
                        }
                    }
                    break;
                case etouchState.Object:
                    if(currentTouchDic.TryGetValue(touchId,out ITouchable value))
                        value?.OnTouchHold(position);
                    break;
            }
        }
    }

    private void OnTouchCanceled(Finger finger)
    {

        Vector2 position = finger.screenPosition;
        int touchId = finger.index;
        switch (touchState)
        {
            case etouchState.Player:
                if(isMoving)
                {
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
                        StartCoroutine(EnableEventSystem());
                    }
                }
                break;
            case etouchState.UI:
                if (currentUIDic.Remove(touchId, out IUITouchable touch))
                {
                    PointerEventData pointerData = new PointerEventData(eventSystem)
                    {
                        position = position
                    };
                    touch?.OnUIEnd(pointerData);
                }
                UIID.Remove(touchId);
                if (UIID.Count.Equals(0))
                {
                    touchState = etouchState.Normal;
                }
                break;
            case etouchState.Object:
                if (currentTouchDic.Remove(touchId, out ITouchable value))
                {
                    value?.OnTouchEnd(position);
                }
                if (currentTouchDic.Count.Equals(0))
                {
                    touchState = etouchState.Normal;
                    StartCoroutine(EnableEventSystem());
                }
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool IsTouchOnUI(int touchId, Vector2 position)
    {
        if (eventSystem.enabled)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = position
            };

            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, results);
            if(results.Count > 0)
            {
                if (results[0].gameObject.TryGetComponent(out IUITouchable touchUI))
                {
                    if (!currentUIDic.ContainsValue(touchUI))
                    {
                        currentUIDic.Add(touchId, touchUI);
                        currentUIDic[touchId].OnUIStarted(pointerData);
                    }

                }
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
    private bool IsTouchableObjectAtPosition(int touchId, Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        int filteredLayer = ~ignoreLayer.value;
        //레이케스트가 터치가능 오브젝트에 충돌했거나 어디에도 충돌되지 않았을 때
        if (Physics.Raycast(ray, out hit, touchDistance, filteredLayer))
        {
            if (hit.collider == null) return false;

            int hitLayer = hit.collider.gameObject.layer;

            // `touchableObjectLayer`에 hit 객체의 레이어가 포함되어 있는지 확인
            bool isTouchableObject = (touchableObjectLayer.value & (1 << hitLayer)) != 0;

            if (isTouchableObject)
            {
                if (hit.collider.TryGetComponent(out ITouchable touchable))
                {
                    if (currentTouchDic.ContainsValue(touchable)) return false;

                    currentTouchDic.Add(touchId, touchable);

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
    public void OnEnableTouchAction()
    {
        if (isTouchSupportEnabled) return;

        isTouchSupportEnabled = true;

        Debug.Log("추가됨");
        touchState = etouchState.Normal;
        

        moveId = -1;
        lookId = -1;
        isMoving = true;
        isTouching = true;
        currentTouchDic = new Dictionary<int, ITouchable>();
        currentUIDic = new Dictionary<int, IUITouchable>();
        UIID = new HashSet<int>();

        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnTouchStarted;
        ETouch.Touch.onFingerMove += OnTouchPerformed;
        ETouch.Touch.onFingerUp += OnTouchCanceled;
    }
    public void OnDisableTouchAction()
    {
        if (!isTouchSupportEnabled) return;

        isTouchSupportEnabled = false;

        ETouch.Touch.onFingerDown -= OnTouchStarted;
        ETouch.Touch.onFingerMove -= OnTouchPerformed;
        ETouch.Touch.onFingerUp -= OnTouchCanceled;
        EnhancedTouchSupport.Disable();
    }

    public void EnableMoveHandler(bool dontTouch)
    {
        isMoving = dontTouch;
        if (!isMoving)
        {
            if (touchState.Equals(etouchState.Player))
            {
                StartCoroutine(EnableEventSystem());
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

    public void EnableTouchHandle(bool dontTouch)
    {
        isTouching = dontTouch;
        if(!isTouching)
        {
            switch(touchState)
            {
                case etouchState.Player:
                    StartCoroutine(EnableEventSystem());
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
                    break;
                case etouchState.Object:
                    currentTouchDic.Clear();
                    touchState = etouchState.Normal;
                    StartCoroutine(EnableEventSystem());
                    break;
            }
        }
    }
    private IEnumerator EnableEventSystem()
    {
        yield return null;
        eventSystem.enabled = true;






    }
}

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
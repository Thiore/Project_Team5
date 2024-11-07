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

    private HashSet<int> activeTouchID;// 활성화된 터치 ID 추적
    private int moveID;
    private int lookID;

    [SerializeReference] private float touchDistance;
    

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
        currentTouchDic = new Dictionary<int, ITouchable>();
        activeTouchID = new HashSet<int>();
        touchState = etouchState.Normal;
        moveID = -1;
        lookID = -1;
    }

    private void Start()
    {
        if(touchDistance>10f||touchDistance<1f)
        {
            touchDistance = 3f;
        }

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
                if (IsTouchOnUI(touchId) && (touchState.Equals(etouchState.Normal) || touchState.Equals(etouchState.UI)))
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
                else if (IsTouchableObjectAtPosition(touchId, position) &&
                         (touchState.Equals(etouchState.Normal) ||
                         touchState.Equals(etouchState.Object)))
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

                if (activeTouchID.Contains(touchId))
                {
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
                            currentTouchDic[touchId].OnTouchHold(position);
                            break;
                    }
                }
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {

                if (activeTouchID.Contains(touchId))
                {
                    switch (touchState)
                    {
                        case etouchState.Player:
                            if (moveID.Equals(touchId))
                            {
                                OnMoveEnd?.Invoke(position);
                                activeTouchID.Remove(touchId);
                                moveID = -1;
                            }
                            if (lookID.Equals(touchId))
                            {
                                activeTouchID.Remove(touchId);
                                OnLookEnd?.Invoke(position);
                                lookID = -1;
                            }
                            if (moveID.Equals(-1) && lookID.Equals(-1))
                            {
                                touchState = etouchState.Normal;
                            }
                            break;
                        case etouchState.UI:
                            activeTouchID.Remove(touchId);
                            if (activeTouchID.Count.Equals(0))
                            {
                                touchState = etouchState.Normal;
                            }

                            break;
                        case etouchState.Object:
                            currentTouchDic[touchId].OnTouchEnd(position);
                            currentTouchDic.Remove(touchId);
                            activeTouchID.Remove(touchId);
                            if(currentTouchDic.Count.Equals(0))
                            {
                                touchState = etouchState.Normal;
                            }
                            break;
                    }
                }
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
    /// 터치한 오브젝트가 터치가능한 오브젝트인지 검사합니다.
    /// </summary>
    /// <param name="touchPosition">adfs</param>
    /// <returns></returns>
    private bool IsTouchableObjectAtPosition(int touchId, Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f, touchableObjectLayer))
        {
            if(hit.collider.TryGetComponent(out ITouchable touchable))
            {
                if (hit.collider.CompareTag("touchableobject"))
                {
                    currentTouchDic.Add(touchId, touchable);
                    currentTouchDic[touchId].OnTouchStarted(touchPosition);
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
    }
    public void OnDisableTuchAction()
    {
        touchAction.Disable();
    }

    
    
}
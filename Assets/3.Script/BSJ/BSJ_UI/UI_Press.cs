using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Press : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputAction press;
    private InputAction screenPos;
    private Vector3 curScreenPos; //현재 Touch Position 저장
    private Vector3 worldPos
    {
        get
        {
            //screen point를 world point로 변환
            //현재 화면위치를 월드 위치로 변환하고 깊이(오브젝트에서 카메라까지의 거리)를 전달, 

            float z = UICam_Touch.WorldToScreenPoint(transform.position).z;
            return UICam_Touch.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
        }
    }
    
    

    private Camera UICam_Touch;
    private bool isDragging; //  Drag상태 인지
    private bool isTouchedOn //현재 Touch 상태
    {
        get
        {
            Ray ray = UICam_Touch.ScreenPointToRay(curScreenPos); //Camera에서 Ray를 쏘아 현재 화면 위치 알기
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // hit에 item이 맞는지 확인 후 맞다면 true
            {
                return hit.transform == transform; 
            }
            return false; //아니라면 false 반환
            
        }
    }

    private void Awake()
    {
        UICam_Touch = Camera.main;
        //Action Map 가져오기
        var uiActionMap = inputActionAsset.FindActionMap("UI");
        press = uiActionMap.FindAction("Press");
        screenPos = uiActionMap.FindAction("Screen Position");

        press.Enable();
        screenPos.Enable();

        //화면 위치 변경될 때 마다 콜백(performed)context로 얻은 정보를 체크
        //                                   ┌> 현재 화면 위치를 터치의 위치로 Vector2 값으로 Update
        screenPos.performed += context_Screen => { curScreenPos = context_Screen.ReadValue<Vector2>(); };

        //Drag_co 시작
        press.performed += _ => { if(isTouchedOn) StartCoroutine(Drag_co()); };
        press.canceled += _ => { isDragging = false; }; //Touch 멈추면 false 반환????
    }

    //Drag 시작
    private IEnumerator Drag_co()
    {
        isDragging = true;
        Vector3 offset = transform.position - worldPos;
        
        //Grab
        while (isDragging)
        {
            //Dragging

            transform.position = worldPos + offset;
            yield return null;
        }
        //Drop
    }
}

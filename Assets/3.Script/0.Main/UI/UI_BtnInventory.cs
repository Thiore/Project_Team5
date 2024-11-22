 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;

public class UI_BtnInventory : MonoBehaviour
{
    [SerializeField] private GameObject btn;
    [SerializeField] private GameObject inventory;
    [SerializeField] private Toggle quickSlot;
    [Header("3D UI")]
    [SerializeField] private Camera UICam_3D; // 확대 후 단서고정 캡처 위한 Camera
    [SerializeField] private RenderTexture renderTexture; // 확대 한 오브젝트 중 Pin고정 이미지를 출력하기 위한 Render Texture
    [SerializeField] private GameObject Panel3D;
    [SerializeField] private GameObject isPin;
    [SerializeField] private GameObject pinExit;
    [SerializeField] private GameObject pinObj;
    [SerializeField] private GameObject keyClue;
    
    private Animator quickSlotAnim;

    private Toggle pinToggle = null; // 3D UI에서 Pin을 위해 사용

    //inspector창에서 확인하기 위해 public사용 아래 내용 구현 이후 private으로 변경
    public bool isPinItem = false; // 오브젝트가 단서 아이템인지 아닌지 구분



    private void Start()
    {
        quickSlot.TryGetComponent(out quickSlotAnim);
        isPin.TryGetComponent(out pinToggle);

        //임시
        isPinItem = true;
    }
    public void OpenInventory()
    {
        //GameManager.Instance.isInput = true;
            btn.SetActive(false);        
        
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        //GameManager.Instance.isInput = false;
        btn.SetActive(true);
        
        inventory.SetActive(false);
    }

    public void QuickSlot()
    {
        if(quickSlot.isOn)
        {
            quickSlotAnim.SetTrigger("Close");
        }
        else
        {
            quickSlotAnim.SetTrigger("Open");
        }
    }

    public void OnScaleItem(/*인벤토리에 열려있는 아이템 넣어주세요*/)
    {
        Panel3D.SetActive(true);
        pinObj.SetActive(true);
        //현재는 Scale버튼의 OnClick에 넣어놨습니다.
        if (isPinItem/*아이템이 단서아이템이라면 true반환되게하고 isPin은 임시*/)
        {
            isPinItem = true;
            isPin.SetActive(true);
            if (pinToggle.isOn)
            {
                keyClue.SetActive(true);
            }
            else
            {
                keyClue.SetActive(false);
            }
        }
        else
        {
            isPinItem = false;
            pinToggle.isOn = false;
            isPin.SetActive(false);
            keyClue.SetActive(false);
        }

        if (keyClue.activeInHierarchy)
        {
            //단서고정이 켜져있을 때 다른 단서 오브젝트를 켜는 경우를 위해 사용
            PinCapture();
        }
    }

    public void IsPin()
    {
        if (pinToggle.isOn)
        {
            keyClue.SetActive(true);
        }
        else
        {
            keyClue.SetActive(false);
        }
    }

    public void PinCapture()
    {
        if(keyClue.activeInHierarchy&&isPinItem)
        {
            isPin.SetActive(false);
            pinExit.SetActive(false);
            UICam_3D.targetTexture = renderTexture; // 3D UI카메라의 Output Texture 추가
            UICam_3D.Render(); // OutputTexture에 UICam 장면 출력
            UICam_3D.targetTexture = null; // OutputTexture 제거
            isPin.SetActive(true);
            pinExit.SetActive(true);
        }
        
    }
}

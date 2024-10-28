using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("3D UI")]
    [SerializeField] private Camera UICam_3D; // 확대 후 오브젝트 렌더를 위한 Camera
    [SerializeField] private RenderTexture renderTexture; // 확대 한 오브젝트 중 Pin고정 이미지를 출력하기 위한 Render Texture

    private Toggle pinToggle = null; // 3D UI에서 Pin을 위해 사용

    //inspector창에서 확인하기 위해 public사용 아래 내용 구현 이후 private으로 변경
    public bool isPinItem = false; // 오브젝트가 단서 아이템인지 아닌지 구분
    
    [SerializeField] private GameObject pinObj;

    private void Awake()
    {
        pinObj.TryGetComponent(out pinToggle);
        
        //임시
        isPinItem = true;
    }

    public void PinItem(/*인벤토리에 열려있는 아이템 넣어주세요*/)
    {
        //현재는 Scale버튼의 OnClick에 넣어놨습니다.
        
         if(isPinItem/*아이템이 단서아이템이라면 true반환되게하고 isPin은 임시*/)
         {
            isPinItem = true; 
            pinObj.SetActive(true);

         }
         else
         {
            if (pinToggle.isOn)
            { 
                //단서고정 끄기
            }
            isPinItem = false;
            pinObj.SetActive(false);
         }
         
         if(isPinItem) 
        {
            //단서고정이 켜져있을 때 다른 단서 오브젝트를 켜는 경우를 위해 사용
            PinCapture();
        }

    }


    public void PinCapture()
    {
        UICam_3D.targetTexture = renderTexture; // 3D UI카메라의 Output Texture 추가
        UICam_3D.Render(); // OutputTexture에 UICam 장면 출력
        UICam_3D.targetTexture = null; // OutputTexture 제거
    }
}

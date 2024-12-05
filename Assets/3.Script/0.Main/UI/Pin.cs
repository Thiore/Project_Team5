using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Pin : MonoBehaviour, IUITouchable
{
    private Animator anim;
    [Header("3D UI")]
    [SerializeField] private Camera UICam_3D; // 확대 후 단서고정 캡처 위한 Camera
    [SerializeField] private RenderTexture renderTexture; // 확대 한 오브젝트 중 Pin고정 이미지를 출력하기 위한 Render Texture
    [SerializeField] private GameObject keyClue;
    private GameObject dontClue;
    private void Awake()
    {
        TryGetComponent(out anim);
        
    }
    private void OnEnable()
    {
        dontClue = transform.GetChild(0).gameObject;
        if (keyClue.activeInHierarchy)
        {
            dontClue.SetActive(false);
        }
        else
        {
            dontClue.SetActive(true);
        }


    }



    public void OnUIStarted(PointerEventData data)
    {

    }
    public void OnUIHold(PointerEventData data)
    {
        
    }

    public void OnUIEnd(PointerEventData data)
    {
        if (dontClue.activeInHierarchy)
        {
            dontClue.SetActive(true);
            keyClue.SetActive(false);
        }
        else
        {
            dontClue.SetActive(false);
            UICam_3D.targetTexture = renderTexture; // 3D UI카메라의 Output Texture 추가
            UICam_3D.Render(); // OutputTexture에 UICam 장면 출력
            UICam_3D.targetTexture = null; // OutputTexture 제거
            keyClue.SetActive(true);
            ClueItem.Instance.SetClueId();
        }


    }
}

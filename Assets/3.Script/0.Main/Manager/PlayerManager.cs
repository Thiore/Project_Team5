using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; } = null;

    //아이템 Information
    [SerializeField] private UI_ItemInformation itemInfo;
    public UI_ItemInformation ui_iteminfo {  get => itemInfo; }

    public GameObject resetCam;

    [SerializeField] private GameObject btnList;
    public GameObject getBtnList { get => btnList; }

    [SerializeField] private TMP_Text itemName;//인벤토리 아이템 이름 띄울 TextMeshPro
    public TMP_Text getItemName { get => itemName; }

    [SerializeField] private TMP_Text explanation;//인벤토리 아이템 설명 띄울 TextMeshPro
    public TMP_Text getExplanation { get => explanation; }

    public Transform mainPlayer;
    public Transform playerCam;

    private int btnCount;

    private void Awake()
    {
        Instance = this;
        btnCount = 0;
        
    }

    public void SetBtn(bool isOn)
    {
        if(!isOn)
        {
            if (btnCount.Equals(0))
            {
                btnList.SetActive(false);
            }
           
            btnCount += 1;
        }
        else
        {
            btnCount -= 1;
            if (btnCount.Equals(0))
            {
                btnList.SetActive(true);
            }
        }
        
    }
    
    public void ResetCamOff()
    {
        StartCoroutine(ResetCamOff_co());
    }
    private IEnumerator ResetCamOff_co()
    {
        yield return new WaitForSeconds(0.1f);
        resetCam.SetActive(false);
    }

}

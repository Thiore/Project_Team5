using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    //인벤토리
    [SerializeField] private UI_Inventory inventory;
    public UI_Inventory ui_inventory { get => inventory; }

    //아이템 Information
    [SerializeField] private UI_ItemInformation itemInfo;
    public UI_ItemInformation ui_iteminfo {  get => itemInfo; }

    //퀵슬롯 및 
    [SerializeField] private UseButton quickSlot;
    public UseButton getQuickSlot { get => quickSlot; }
    public Transform playerInterface { get => quickSlot.transform.parent; }


    [SerializeField] private UI_LerpImage lerpimage;
    public UI_LerpImage ui_lerpImage { get => lerpimage; }


    [SerializeField] private GameObject btnList;
    public GameObject getBtnList { get => btnList; }

    [SerializeField] private TMP_Text itemName;//인벤토리 아이템 이름 띄울 TextMeshPro
    public TMP_Text getItemName { get => itemName; }

    [SerializeField] private TMP_Text explanation;//인벤토리 아이템 설명 띄울 TextMeshPro
    public TMP_Text getExplanation { get => explanation; }



    #region addTeo
    [SerializeField] private TeoLerp lerpImage;
    public TeoLerp getLerpImage { get => lerpImage; }

    public Transform mainPlayer;
    public Transform playerCam;

    public Light flashLight;
    [SerializeField] private Inventory inven;
    public Inventory getInven { get => inven; }
    #endregion


    public Button optionBtn;

    private void Awake()
    {
        Instance = this;

        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; } = null;

    [SerializeField] private UI_Inventory inventory;
    public UI_Inventory ui_inventory { get => inventory; }

    [SerializeField] private UI_ItemInformation itemInfo;
    public UI_ItemInformation ui_iteminfo {  get => itemInfo; }
    [SerializeField] private GameObject playerInterface;
    public GameObject getInterface { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

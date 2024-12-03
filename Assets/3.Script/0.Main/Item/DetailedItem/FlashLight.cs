using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour, IUseTrigger
{
    [SerializeField] private int id;
    public Item item { get; private set; }
    private Light flashLight;
    private void Awake()
    {
        TryGetComponent(out flashLight);
    }
    private void OnEnable()
    {
        if (DataSaveManager.Instance.GetItemState(id))
        {
            TriggerButton.OnUseTrigger += OnUseTrigger;
            Debug.Log("추가됨");
        }
       
        
    }

    private void OnDestroy()
    {
        TriggerButton.OnUseTrigger -= OnUseTrigger;
    }
    public void OnUseTrigger(Item item)
    {
        if(item.id.Equals(id))
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }

    public void SetUseFlashLight()
    {
        TriggerButton.OnUseTrigger += OnUseTrigger;
    }    
}

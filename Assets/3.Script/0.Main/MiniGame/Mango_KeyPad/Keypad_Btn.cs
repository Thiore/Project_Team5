using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Keypad_Btn : MonoBehaviour, IUITouchable
{
    [SerializeField] private InteractionKeyPad keyPad;
    private TMP_Text text;
    private void Awake()
    {
        transform.GetChild(0).TryGetComponent(out text);
    }
    public void OnUIEnd(PointerEventData data)
    {
        keyPad.BtnClick(text);
    }

    public void OnUIHold(PointerEventData data)
    {
        
    }

    public void OnUIStarted(PointerEventData data)
    {
        
    }
}

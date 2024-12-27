using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabletDialogue : MonoBehaviour, IUITouchable
{
    [SerializeField] private TabletMonitor tabletMonitor;

    public void OnUIStarted(PointerEventData data)
    {
    }

    public void OnUIHold(PointerEventData data)
    {
    }

    public void OnUIEnd(PointerEventData data)
    {
        tabletMonitor.OnCut();
    }
}

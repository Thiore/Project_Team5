using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPuzzleCanvas : MonoBehaviour,ITouchable
{
    [SerializeField] private GameObject missionStart;
    [SerializeField] private GameObject missionExit;

    [SerializeField] private GameObject btnExit;
    private Collider mask;
    [HideInInspector]
    public bool isClear;

    

    private void Start()
    {
        TryGetComponent(out mask);
        isClear = false;
    }

    
    public void OffKeypad()
    {
        missionExit.SetActive(true);
        missionStart.SetActive(false);
        mask.enabled = true;
        isClear = true;
    }

    public void OnTouchStarted(Vector2 position)
    {
        if (isClear) return;

        missionStart.SetActive(true);

        mask.enabled = false;

        btnExit.SetActive(true);
    }

    public void OnTouchHold(Vector2 position)
    {
        
    }

    public void OnTouchEnd(Vector2 position)
    {
        
    }
}

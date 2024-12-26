using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBasic : InteractionOBJ, ITouchable
{

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        if(anim == null)
        {
            if (!TryGetComponent(out anim))
            {
                transform.parent.TryGetComponent(out anim);
            }
        }
    }
    public void OnTouchEnd(Vector2 position)
    {

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if(hit.collider.gameObject.Equals(gameObject))
            {
                
                isTouching = !isTouching;
                
                anim.SetBool(openAnim, isTouching);
                if(normalCamera!= null)
                {
                    normalCamera.SetActive(isTouching);
                    if (PlayerManager.Instance != null)
                    {
                        PlayerManager.Instance.SetBtn(!isTouching);
                    }
                    if(TouchManager.Instance != null)
                    {
                        TouchManager.Instance.EnableMoveHandler(!isTouching);
                    }
                }
            }
        }
    }
    public void OnTouchStarted(Vector2 position)
    {
    }
    public void OnTouchHold(Vector2 position)
    {
    }

    

    

}

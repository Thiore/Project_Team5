 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class DoorBasic : InteractionOBJ, ITouchable
{

    protected override void Start()
    {
        base.Start();
        isTouching = false;
        
        if (!TryGetComponent(out anim))
        {
            transform.parent.TryGetComponent(out anim);
        }
        Debug.Log(anim.name);
    }
    public void OnTouchEnd(Vector2 position)
    {

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if(hit.collider.gameObject.layer.Equals(gameObject.layer))
            {
                
                isTouching = !isTouching;
                if(normalCamera != null)
                {
                    normalCamera.SetActive(isTouching);
                    if(isTouching)
                    {
                        TouchManager.Instance.EnableMoveHandler(false);
                    }
                    else
                    {
                        TouchManager.Instance.EnableMoveHandler(true);
                    }
                }
                    

                anim.SetBool(openAnim, isTouching);
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

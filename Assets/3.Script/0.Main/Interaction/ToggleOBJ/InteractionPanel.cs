using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPanel : TouchPuzzleCanvas
{
    public override void OnTouchEnd(Vector2 position)
    {
        anim.SetBool(openAnim, true);
    }

    protected override void ClearEvent()
    {
        
    }

    protected override void ResetCamera()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSwitch : InteractionOBJ, ITouchable
{
    [SerializeField] private Lamp[] lamps;
    private bool isClear;

    public void OnTouchEnd(Vector2 position)
    {

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, TouchManager.Instance.getTouchDistance, TouchManager.Instance.getTouchableLayer))
        {
            if (hit.collider.gameObject.Equals(gameObject))
            {
                if (!isClear)
                {
                    if (DataSaveManager.Instance.GetGameState(1, 103))
                    {
                        isClear = true;
                    }
                    else
                    {
                        DialogueManager.Instance.TalkStoryStart(3, 3, "Table_StoryB1", false);
                    }
                }

                if (isClear)
                {
                    isTouching = anim.GetBool(openAnim);
                    anim.SetBool(openAnim, !isTouching);
                    foreach(Lamp lamp in lamps)
                    {
                        lamp.OnLamp(false);
                    }
                }
            }
        }
    }

    public void OnTouchHold(Vector2 position)
    {
       
    }

    public void OnTouchStarted(Vector2 position)
    {
        
    }
}

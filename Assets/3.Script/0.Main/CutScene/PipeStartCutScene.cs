using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeStartCutScene : MonoBehaviour
{
    [SerializeField] private Animator[] interactionAnim;
    [SerializeField] private GameObject[] interactionCam;
    [SerializeField] private GameObject ToggleButton;
    private Image logo;

    private void Awake()
    {
        TryGetComponent(out logo);
    }

    private void OnEnable()
    {
        StartCoroutine(PipeStory());
    }
    private IEnumerator PipeStory()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime*0.1f;
            logo.color = new Color(1f, 1f, 1f, Mathf.Clamp(alpha,0f, 1f));
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        
        while(alpha>0f)
        {
            alpha -= Time.deltaTime * 0.1f;
            logo.color = new Color(1f, 1f, 1f, Mathf.Clamp(alpha, 0f, 1f));
            yield return null;
        }
        ToggleButton.SetActive(true);
        if (TabletDialogue.Instance != null)
            TabletDialogue.Instance.SetDialogueIndex(100, 117);
        else
            Debug.Log("아직 생성안됨");
        gameObject.SetActive(false);
    }
    
}

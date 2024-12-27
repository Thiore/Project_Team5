using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeStartCutScene : MonoBehaviour
{
    [SerializeField] private Animator[] interactionAnim;
    [SerializeField] private GameObject[] interactionCam;
    [SerializeField] private FixPipeGameManager pipeManager;
    [SerializeField] private TabletMonitor tabletMonitor;

    private readonly int openAnim = Animator.StringToHash("Open");
    
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
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(false);
        }
        TouchManager.Instance.EnableMoveHandler(false);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            logo.color = new Color(1f, 1f, 1f, Mathf.Clamp(alpha,0f, 1f));
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        
        while(alpha>0f)
        {
            alpha -= Time.deltaTime;
            logo.color = new Color(1f, 1f, 1f, Mathf.Clamp(alpha, 0f, 1f));
            yield return null;
        }
        tabletMonitor.gameObject.SetActive(true);
        tabletMonitor.SetDialogueIndex(100, 117);
        while(tabletMonitor.isDialogue)
        {
            yield return null;
        }

        if (!interactionAnim[1].GetBool(openAnim))
            interactionAnim[1].SetBool(openAnim, true);

        interactionCam[0].SetActive(true);
        interactionCam[1].SetActive(true);
        Invoke("StartCam", 3f);
    }
    private void StartCam()
    {
        interactionAnim[0].SetBool(openAnim, true);
        pipeManager.GameStart();
        Invoke("NextCamMove", 2f);
    }
    private void NextCamMove()
    {
        interactionCam[0].SetActive(false);

        Invoke("EngineRoomMove", 3f);
    }
    private void EngineRoomMove()
    {
        interactionCam[2].SetActive(true);
        interactionAnim[2].SetBool(openAnim, true);
        interactionCam[1].SetActive(false);

        Invoke("ResetCamera", 6f);
    }

    private void ResetCamera()
    {
        PlayerManager.Instance.resetCam.SetActive(true);
        interactionCam[2].SetActive(false);
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetBtn(true);
        }
        TouchManager.Instance.EnableMoveHandler(true);
        PlayerManager.Instance.ResetCamOff();
    }
}

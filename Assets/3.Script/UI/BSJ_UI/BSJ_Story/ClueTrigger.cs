using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueTrigger : MonoBehaviour
{
    [SerializeField] private int clueIndex;

    private GameObject playInterface;
    private GameObject clue;
    private GameObject clueItem;
    private GameObject exit;

    private ReadInputData input;

    private void Start()
    {

        FindObjectUI();

        //ReadInputData 가져오기
        TryGetComponent(out input);

    }

    private void Update()
    {
        if (input.isTouch)
        {
            GetClue();
        }
    }

    //단서 오브젝트 얻었을 때, 3D_UI 활성화
    private void GetClue()
    {
        //기본 UI 비활성화
        playInterface.gameObject.SetActive(false);

        //3D_UI 활성화
        exit.gameObject.SetActive(true);
        clueItem.gameObject.SetActive(true);

    }

    public void Exit3D_UI()
    {
        //3D_UI 비활성화
        exit.gameObject.SetActive(false);
        clueItem.gameObject.SetActive(false);

        //기본 UI 활성화
        playInterface.gameObject.SetActive(true);
    }

    private void FindObjectUI()
    {


        if (clue != null)
        {
            Transform clueItem_ = clue.transform.GetChild(clueIndex);
            clueItem = clueItem_.gameObject;

            Transform exit_ = clue.transform.GetChild(6);
            exit = exit_.gameObject;
        }
    }

    private void OnEnable()
    {
        playInterface = GameObject.FindGameObjectWithTag("PlayInterface");
        clue = GameObject.FindGameObjectWithTag("Clue");
    }
}

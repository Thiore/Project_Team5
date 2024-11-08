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

    //스토리 오브젝트 => 해당 위치에서 반복적으로 터치할 수 있게 / 삭제 (x)
    public void ExitStoryObj()
    {
        //3D_UI 비활성화
        exit.gameObject.SetActive(false);
        clueItem.gameObject.SetActive(false);

        //기본 UI 활성화
        playInterface.gameObject.SetActive(true);
        
    }

    //단서 오브젝트 => 인벤토리로 들어가기 때문에 / 삭제(O)
    public void ExitClueObj()
    {
        //3D_UI 비활성화
        exit.gameObject.SetActive(false);
        clueItem.gameObject.SetActive(false);

        //기본 UI 활성화
        playInterface.gameObject.SetActive(true);

        //오브젝트 삭제
        Destroy(gameObject);
    }

    private void FindObjectUI()
    {


        if (clue != null)
        {
            Transform clueItem_ = clue.transform.GetChild(clueIndex);
            clueItem = clueItem_.gameObject;

            Transform exit_ = clue.transform.GetChild(3);
            exit = exit_.gameObject;
        }
    }

    private void OnEnable()
    {
        playInterface = GameObject.FindGameObjectWithTag("PlayInterface");
        clue = GameObject.FindGameObjectWithTag("Clue");
    }
}

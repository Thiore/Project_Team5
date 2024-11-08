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
    private GameObject uiCamera;

    private ReadInputData input;

    private void Start()
    {

        

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
        uiCamera.gameObject.SetActive(true);

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
        uiCamera.gameObject.SetActive(false);

    }

    //단서 오브젝트 => 인벤토리로 들어가기 때문에 / 삭제(O)
    public void ExitClueObj()
    {
        //3D_UI 비활성화
        exit.gameObject.SetActive(false);
        clueItem.gameObject.SetActive(false);

        //기본 UI 활성화
        playInterface.gameObject.SetActive(true);
        uiCamera.gameObject.SetActive(false);


    }

    private void FindObjectUI()
    {


        if (clue != null)
        {
            Transform clueItem_ = clue.transform.GetChild(clueIndex);
            clueItem = clueItem_.gameObject;

            Transform exit_ = clue.transform.GetChild(3);
            exit = exit_.gameObject;

            //임시로 5번 째에서 찾기, 테스트 끝나면 카메라 상단으로 올리고 clueIndex + 1 로
            Transform camera_ = clue.transform.GetChild(4);
            uiCamera = camera_.gameObject;
        }
    }

    private void OnEnable()
    {
        playInterface = GameObject.FindGameObjectWithTag("PlayInterface");
        clue = GameObject.FindGameObjectWithTag("Clue");
        FindObjectUI();
    }
}

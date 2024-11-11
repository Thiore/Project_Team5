using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueTrigger : MonoBehaviour, ITouchable
{
    [SerializeField] private int clueIndex;

    private GameObject mainPlayer;
    private GameObject playInterface;
    private GameObject clue;
    private GameObject clueItem;
    private GameObject exit;
    private GameObject uiCamera;
    private GameObject uiCanvas;


    //단서 오브젝트 얻었을 때, 3D_UI 활성화
    private void GetClue()
    {
        Debug.Log("여기");
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
        mainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (mainPlayer != null)
        {
            //UI_Camera 찾기
            Transform UI_Camera = mainPlayer.transform.GetChild(2);
            uiCamera = UI_Camera.gameObject;

            //Clue 찾기
            Transform Clue_ = mainPlayer.transform.GetChild(3);
            clue = Clue_.gameObject;

            //UI_Canvas의 Exit 찾기
            Transform UI_Canvas = mainPlayer.transform.GetChild(4);
            uiCanvas = UI_Canvas.gameObject;

            //Canvas의 PlayInterface찾기
            Transform PlayInterface_ = mainPlayer.transform.GetChild(5);
            playInterface = PlayInterface_.gameObject;

            if (clue != null)
            {
                Transform clueItem_ = clue.transform.GetChild(clueIndex);
                clueItem = clueItem_.gameObject;

                Transform exit_ = uiCanvas.transform.GetChild(0);
                exit = exit_.gameObject;
            }
        }


    }

    private void OnEnable()
    {
        FindObjectUI();
    }

    public void OnTouchStarted(Vector2 position)
    {
        GetClue();
    }

    public void OnTouchHold(Vector2 position)
    {
    }

    public void OnTouchEnd(Vector2 position)
    {
        
    }
}

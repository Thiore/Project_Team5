using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    private SaveManager instance = null;
    public SaveManager Instance { get; private set; }

    private string savePath; //저장 파일 경로
    public StateData.GameState gameState; //게임 상태 관리 객체

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instance = instance;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Json파일 저장 경로
        savePath = Path.Combine(Application.persistentDataPath, "gameState.json");

        //게임 상태 초기화 (층 리스트 초기화)
        gameState = new StateData.GameState { floors = new List<StateData.FloorState>() };
    }

    //새게임
    public void NewGame()
    {
        //gameState 초기화 (층 리스트)
        gameState = new StateData.GameState
        {
            floors = new List<StateData.FloorState>()
        };

        // 각 층과 상호작용 오브젝트 초기화 및 기본 상태 설정 (임시로 4 해놨음)
        for (int floorIndex = 0; floorIndex < 4; floorIndex++)
        {
            // 층 정보를 초기화 후 층 인덱스 및 오브젝트 상태 리스트 설정
            StateData.FloorState floor = new StateData.FloorState
            {
                //현재 층 인덱스 설정
                floorIndex = floorIndex,
                //층 내 오브젝트 리스트 초기화
                interactableObjects = new List<StateData.InteractableObjectState>()
            };

            // 각 층 상호작용 오브젝트 초기화 (임시로 5 해놨음)
            for (int objectIndex = 0; objectIndex < 5; objectIndex++)
            {
                //오브젝트 상태 초기화(상호작용되지 않은 상태로)
                StateData.InteractableObjectState objState = new StateData.InteractableObjectState
                {
                    //오브젝트 인덱스 설정
                    objectIndex = objectIndex,
                    //상호작용되지 않은 상태
                    isInteracted = false
                };
                //초기화된 오브젝트 상태를 층에 추가
                floor.interactableObjects.Add(objState);
            }
            //초기화 된 층 상태를 게임 상태에 추가
            gameState.floors.Add(floor);
        }

    }

    // 게임 로드
    public void LoadGameState()
    {
        //저장된 Json파일이 존재 하는 경우 로드
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            //Json 문자열을 GameState 객체로 할당
            gameState = JsonConvert.DeserializeObject<StateData.GameState>(json);
        }
    }

    // 게임 상태 저장
    public void SaveGameState()
    {
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    // 상태 업데이트 (층 및 오브젝트 상태 업데이트)
    public void UpdateObjectState(int floorIndex, int objectIndex, bool isInteracted)
    {
        //해당 층을 찾거나 새로 생성(새게임)
        StateData.FloorState floor = gameState.floors.Find(f => f.floorIndex == floorIndex);

        //해당 층이 없을 경우 새로운 층 추가
        if (floor == null)
        {
            floor = new StateData.FloorState
            {
                //층 인덱스 설정
                floorIndex = floorIndex,
                //오브젝트 리스트 초기화
                interactableObjects = new List<StateData.InteractableObjectState>()
            };
            //생성한 층을 floors 리스트에 추가
            gameState.floors.Add(floor);
        }

        //해당 오브젝트를 찾거나 새로 생성하여 상태 업데이트
        StateData.InteractableObjectState objState = floor.interactableObjects.Find(obj => obj.objectIndex == objectIndex);

        //오브젝트가 존재하지 않을 경우 새로운 로브젝트 추가
        if (objState == null)
        {
            objState = new StateData.InteractableObjectState
            {
                //오브젝트 인덱스 설정
                objectIndex = objectIndex,
                //전달된 상호작용 상태 설정
                isInteracted = isInteracted
            };
            //생성한 오브젝트interactableObjects 리스트에 추가
            floor.interactableObjects.Add(objState);
        }
        else
        {
            //오브젝트가 이미 존재하는 경우 -> 상호작용 상태만 업데이트
            objState.isInteracted = isInteracted;
        }

        //변경된 상태 Json 파일에 저장
        SaveGameState();

    }

    //puzzle과 상호작용하는 door에 상태 알리기
    public bool PuzzleState(int floorIndex, int objectIndex)
    {
        StateData.FloorState floor = gameState.floors.Find(f => f.floorIndex == floorIndex);

        if (floor != null)
        {
            StateData.InteractableObjectState objState = floor.interactableObjects.Find(obj => obj.objectIndex == objectIndex);
            if (objState != null)
            {
                return objState.isInteracted;
            }
        }

        return false;
    }
}

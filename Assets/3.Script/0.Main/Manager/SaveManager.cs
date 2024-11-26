using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; } = null;

    private string savePath; //게임 상태 저장 
    private string itemstatepath; //아이템 상태 저장 
    private GameObject loadGameButton; //이어하기 버튼
    private GameObject mainButton; //Lobby Main Button
    public StateData.GameState gameState; // 게임 상태 데이터
    public Dictionary<int, ItemSaveData> itemsavedata; // 아이템 상태 데이터 저장 딕셔너리
    public string selectedLocale; //현재 선택된 언어
    private bool hasSaveFileCache; //파일 존재여부 한 번만 확인 후 캐싱하기 위해
    public string selectedLocale_ //현재 선택된 언어
    {
        get => gameState.selectedLocale;
        set => gameState.selectedLocale = value;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSaveManager();
            LoadGameState();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void OnEnable()
    {
        //LoadGameButton 찾기
        mainButton = GameObject.FindGameObjectWithTag("GameController");
        Transform loadGame = mainButton.transform.GetChild(0);
        loadGameButton = loadGame.gameObject;

        //초기 저장 파일 상태 확인
        //CheckSaveFile에서 파일 존재 여부를 한 번만 확인 후, 결과를 hasSaveFileCache에 저장
        //이후 캐싱된 값을 참조하여 디스크 접근을 제거 (속도 향상, 효율 Up ???)
        CheckSaveFile();

        if (loadGameButton != null)
        {
            //저장 파일이 있는 경우 버튼 활성화, 없으면 비활성화
            loadGameButton.SetActive(HasSaveFile());
        }
    }

    //SaveManager 초기화
    public void InitializeSaveManager()
    {
        // Json���� ���� ���
        savePath = Path.Combine(Application.persistentDataPath, "gameState.json");
        Debug.Log(savePath);

        itemstatepath = Path.Combine(Application.persistentDataPath, "itemsaveState.json");

        //���� ���� �ʱ�ȭ (�� ����Ʈ �ʱ�ȭ)
        gameState = new StateData.GameState { floors = new List<StateData.FloorState>() };// 기본 게임 상태 초기화
        itemsavedata = new Dictionary<int, ItemSaveData>();// 아이템 상태 초기화
    }

    //유저가 백그라운드로 갔을 때, 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.Save();
            SaveGameState();
        }
    }

    //어플리케이션이 종료 되었을 때
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
        SaveGameState();
    }

    //새게임
    public void NewGame()
    {
        //gameState 초기화 (층 리스트)
        gameState = new StateData.GameState
        {
            floors = new List<StateData.FloorState>(),

            //플레이어 위치 초기화
            playerPositionX = 4.224149f,
            playerPositionY = 1f,
            playerPositionZ = 3.059584f,
            playerRotationX = -0.1819898f,
            playerRotationY = -0.400133f,
            playerRotationZ = 0.08140796f,
            playerRotationW = -0.8945088f// 기본 회전

        };


        // 각 층과 상호작용 오브젝트 초기화 및 기본 상태 설정 (4개의 층) *추가 수정 있으면 바꿀 것
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

            // 각 층 상호작용 오브젝트 초기화 (임시로 5 해놨습니다.)
            for (int objectIndex = 0; objectIndex < 5; objectIndex++)
            {
                //오브젝트 상태 초기화 (상호작용되지 않은 상태로)
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
        NewGameItemData();
    }

    // 이어하기
    public void LoadGameState()
    {
        // 저장된 파일이 존재하면 데이터를 읽어들임
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            //Json 데이터를 GameState 객체로 역직렬화
            gameState = JsonConvert.DeserializeObject<StateData.GameState>(json);

            //저장된 언어 상태 가져오기
            selectedLocale_ = gameState.selectedLocale;
        }

        LoadItemData();
    }

    public void NewGameItemData()
    {
        Debug.Log("새로하기: 아이템 데이터를 초기화합니다.");

        // 데이터를 초기화
        itemsavedata = new Dictionary<int, ItemSaveData>();

        // 새로운 초기 데이터를 저장
        //SaveItemData();
    }

    public void LoadItemData()
    {
        //파일 없으면 새로 만들기 
        if (!File.Exists(itemstatepath))
        {
            Debug.Log("Item save file not found. Initializing empty data.");
            itemsavedata = new Dictionary<int, ItemSaveData>();
            SaveItemData();
            return;
        }

        string itemjson = File.ReadAllText(itemstatepath);
        itemsavedata = JsonConvert.DeserializeObject<ItemSaveData[]>(itemjson).ToDictionary(x => x.id, x => x);
        foreach (KeyValuePair<int, ItemSaveData> data in itemsavedata)
        {
            Debug.Log($"Saving Item - ID: {data.Key}: {data.Value.isused}");
        }
    }

    public void SaveItemData()
    {
        Debug.Log("Saving Item Data...");
        List<ItemSaveData> itemList = itemsavedata.Values.ToList();
        foreach (var item in itemList)
        {
            Debug.Log($"Saving Item - ID: {item.id}, UseState: {item.isused}");
        }

        string itemsjson = JsonConvert.SerializeObject(itemList, Formatting.Indented);
        File.WriteAllText(itemstatepath, itemsjson);
    }

    // 게임 상태 저장
    public void SaveGameState()
    {
        //로컬라이제이션 현재 선택 언어 저장
        gameState.selectedLocale = LocalizationSettings.SelectedLocale.Identifier.Code;


        // 게임 상태를 JSON 형식으로 직렬화하여 파일에 저장
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        File.WriteAllText(savePath, json);

        SaveItemData();

        //저장 후 LoadGameButton 상태 업데이트
        UpdateLoadGameButton();

        hasSaveFileCache = true;

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

        //오브젝트가 존재하지 않은 경우 새로운 오브젝트 추가
        if (objState == null)
        {
            objState = new StateData.InteractableObjectState
            {
                //오브젝트 인덱스 설정
                objectIndex = objectIndex,
                //전달된 상호작용 상태 설정
                isInteracted = isInteracted
            };
            //생성한 오브젝트 interactableObjects 리스트에 추가
            floor.interactableObjects.Add(objState);
        }
        else
        {
            //오브젝트가 이미 존재하는 경우 -> 상호작용 상태만 업데이트
            objState.isInteracted = isInteracted;
        }

        SavePlayerState();
    }

    //puzzle과 상호작용하는 door에 상태 알리기
    public bool PuzzleState(int floorIndex, int objectIndex)
    {
        if (objectIndex.Equals(0))
        {
            return true;
        }
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

    //플레이어 위치
    public void LoadPlayerPosition(Transform player)
    {

        if (player != null)
        {
            player.transform.localPosition = new Vector3(
                gameState.playerPositionX,
                gameState.playerPositionY,
                gameState.playerPositionZ
                );

        }
    }

    //플레이어 회전
    public void LoadPlayerRotation(Transform camera)
    {
        if (camera != null)
        {
            camera.transform.localRotation = new Quaternion(
                gameState.playerRotationX,
                gameState.playerRotationY,
                gameState.playerRotationZ,
                gameState.playerRotationW
                );
        }
    }

    //플레이어의 위치를 받아와서 위치 및 회전 저장
    public void SavePlayerState()
    {
        gameState.playerPositionX = PlayerManager.Instance.mainPlayer.localPosition.x;
        gameState.playerPositionY = PlayerManager.Instance.mainPlayer.localPosition.y;
        gameState.playerPositionZ = PlayerManager.Instance.mainPlayer.localPosition.z;

        gameState.playerRotationX = PlayerManager.Instance.playerCam.localRotation.x;
        gameState.playerRotationY = PlayerManager.Instance.playerCam.localRotation.y;
        gameState.playerRotationZ = PlayerManager.Instance.playerCam.localRotation.z;
        gameState.playerRotationW = PlayerManager.Instance.playerCam.localRotation.w;

    }



    // �� ������ �� ������ �ִ��� Ȯ���ؼ� Json ����ȭ ���ָ� �Ǵ°� �ƴ� ? 
    // ����ȭ�ؼ� �ٸ� ���Ϸ� �������ָ� ���ݾ� 

    // �ٵ� ������ �� �ʿ��� ? 
    // �ε� ���� ���纻 �����ΰ� �� �������鼭 ���� ?

    public void InputItemSavedata(Item item)
    {
        // 여기에 담겼다는거 자체가 이미 얻었기 때문에 isused 여부만 
        if (itemsavedata.ContainsKey(item.id))
        {
            itemsavedata[item.id].isused = !item.isused;
        }
        else
        {
            ItemSaveData data = new ItemSaveData();
            data.id = item.id;
            data.isused = item.isused;
            itemsavedata.Add(item.id, data);
        }
    }

    private void CheckSaveFile()
    {
        hasSaveFileCache = File.Exists(savePath) || File.Exists(itemstatepath);
    }

    //Lobby 씬에서 LoadGameButton 상태 확인
    private bool HasSaveFile()
    {
        return hasSaveFileCache;
    }

    //게임 저장할 때, LoadGameButton 상태 업데이트
    private void UpdateLoadGameButton()
    {
        if (loadGameButton != null)
        {
            loadGameButton.SetActive(HasSaveFile());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class DataSaveManager : MonoBehaviour
{
    public static DataSaveManager Instance { get; private set; } = null;

    //게임 진행 상태 저장 경로
    private string stateSavePath;
    //아이템 획득 및 사용 내역 저장 경로
    private string itemSavePath;

    /// <summary>
    /// 게임의 진행 상태를 저장하는 Dictionary
    /// <para>Dictionary : 키는 층, value는 Dictionary(Key : 오브젝트, value : isInteracted(상호작용여부))</para>
    /// </summary>
    public Dictionary<int, Dictionary<int, bool>> gameStateData { get; private set; }
    private Dictionary<int, Dictionary<int, bool>> tempGameStateData;

    /// <summary>
    /// 아이템의 획득 및 사용 여부를 저장하는 Dictionary
    /// <para>Dictionary : Key : ItemID, value : isUsed(사용여부)</para>
    /// </summary>
    public Dictionary<int, bool> itemStateData { get; private set; }
    private Dictionary<int, bool> tempItemStateData;
    /// <summary>
    /// 아이템의 정보를 저장하는 Dictionary
    /// <para>Dictionary : Key : ItemID, value : Item의 정보</para>
    /// </summary>
    public Dictionary<int, Item> itemData { get; private set; }

    //PlayerPrefabs string값 캐싱
    private readonly string posX = "playerPosX";
    private readonly string posY = "playerposY";
    private readonly string posZ = "playerposZ";
    private readonly string rotX = "playerrotX";
    private readonly string rotY = "playerrotY";
    private readonly string rotZ = "playerrotZ";
    private readonly string rotW = "playerrotW";

    //플레이어 새게임 좌표
    private readonly float newPlayerPositionX = 4.177f;
    private readonly float newPlayerPositionY = 1.25f;
    private readonly float newPlayerPositionZ = 3.52f;
    
    //플레이어 새게임 화전값
    private readonly float newPlayerRotationX = -0.0006117933f;
    private readonly float newPlayerRotationY = 0.7866725f;
    private readonly float newPlayerRotationZ = -0.001418735f;
    private readonly float newPlayerRotationW = -0.6173685f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            tempGameStateData = new Dictionary<int, Dictionary<int, bool>>();
            tempItemStateData = new Dictionary<int, bool>();
            InitializeManager();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Manager가 초기화될 때 로컬에 저장되어 있는 데이터 불러오기
    /// </summary>
    private void InitializeManager()
    {
        stateSavePath = Path.Combine(Application.persistentDataPath, "SaveGameState.json");
        itemSavePath = Path.Combine(Application.persistentDataPath, "SaveItemState.json");
    }

    /// <summary>
    /// 새 게임을 할때 현재 가지고 있는 데이터 삭제
    /// </summary>
    public void NewGame()
    {
        
        gameStateData = new Dictionary<int, Dictionary<int, bool>>();
        itemStateData = new Dictionary<int, bool>();
    }
    //유저가 백그라운드로 갔을 때, 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.Save();
            SaveGame();
        }
    }

    //어플리케이션이 종료 되었을 때
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
        SaveGame();
    }
    /// <summary>
    /// 게임이 정지&종료될 때와 Lobby로 넘어왔을 때 호출하여 
    /// <para>게임에 저장되어있는 데이터를 Json으로 직렬화해 내보내는 메서드</para>
    /// </summary>
    public void SaveGame()
    {
        if (gameStateData.Count > 0|| itemStateData.Count > 0)
        {
            string stateJson = JsonConvert.SerializeObject(gameStateData, Formatting.Indented);
            File.WriteAllText(stateSavePath, stateJson);
            tempGameStateData = gameStateData;

            string itemJson = JsonConvert.SerializeObject(itemStateData, Formatting.Indented);
            File.WriteAllText(itemSavePath, itemJson);
            tempItemStateData = itemStateData;
        }
        
    }

    /// <summary>
    /// gameStateData에 현재 게임 상태 업데이트 (층 및 오브젝트의 상호작용 여부 업데이트)
    /// </summary>
    /// <param name="floorIndex">해당 오브젝트가 상호작용하는 층의 index
    /// <para>B1F : 0, B2F : 1, 1F : 2, 2F이상 : 3, 화물실 : 4</para></param>
    /// <param name="objectIndex">해당 오브젝트의 인덱스
    /// <para>아이템과 상호작용하는 오브젝트 : ItemID와 동일, 게임에 대한 상호작용이 필요한 경우 : 100부터 시작</para></param>
    /// <param name="isInteracted">기본값 true(열려있는걸 다시 잠가야하는 경우 false로 입력</param>
    public void UpdateGameState(int floorIndex, int objectIndex, bool isInteracted = true)
    {
        //해당 층이 없다면 생성
        if (!gameStateData.ContainsKey(floorIndex))
        {
            gameStateData.Add(floorIndex, new Dictionary<int, bool>());
        }

        //해당 오브젝트가 없다면 생성
        if (!gameStateData[floorIndex].ContainsKey(objectIndex))
        {
            gameStateData[floorIndex].Add(objectIndex, isInteracted);
        }
        else
        {
            gameStateData[floorIndex][objectIndex] = isInteracted;
        }

        SavePlayerTransform();

    }

    /// <summary>
    /// itemStateData에 아이템 획득 및 사용을 업데이트
    /// </summary>
    /// <param name="id">아이템의 id값을 입력해주세요</param>
    public void UpdateItemState(int id)
    {
        if(itemStateData.ContainsKey(id))
        {
            itemStateData[id] = true;
        }
        else
        {
            itemStateData.Add(id, false);
        }
        SavePlayerTransform();
    }

    /// <summary>
    /// 미니게임과 상호작용하는 오브젝트가 현재 미니게임이 클리어되어있는지 확인하기 위한 메서드
    /// </summary>
    /// <param name="floorIndex">해당 오브젝트가 상호작용하는 층의 index
    /// <para>B1F : 0, B2F : 1, 1F : 2, 2F이상 : 3, 화물실 : 4</para></param>
    /// <param name="objectIndex">해당 오브젝트의 인덱스
    /// <para>아이템과 상호작용하는 오브젝트 : ItemID와 동일</para>
    /// <para>게임에 대한 상호작용이 필요한 경우 : 100부터 시작</para></param>
    /// <returns>상호작용이 가능한지 불가능한지에 대한 bool값</returns>
    public bool GetGameState(int floorIndex, int objectIndex)
    {
        if (!gameStateData.ContainsKey(floorIndex))
        {
            return false;
        }

        if (!gameStateData[floorIndex].ContainsKey(objectIndex))
        {
            return false;
        }

        return gameStateData[floorIndex][objectIndex];
    }
    /// <summary>
    /// 아이템의 획득과 사용에 대한 정보를 얻기위한 메서드
    /// </summary>
    /// <param name="ID">확인할 아이템의 ID</param>
    /// <param name="isUsed">확인할 아이템의 사용여부
    /// <para>아이템을 사용했는지 사용하지 않았는지 값 반환</para></param>
    /// <returns>true : 아이템 획득, false : 아이템 미획득</returns>
    public bool GetItemState(int ID, out bool isUsed)
    {
        if (ID.Equals(2))//트리거아이템 목록
        {
            isUsed = false;
            return true;
        }
        if(itemStateData.ContainsKey(ID))
        {
            isUsed = itemStateData[ID];
            return true;
        }
        else
        {
            isUsed = false;
            return false;
        }
    }
    /// <summary>
    /// 아이템을 초기화 할 때 획득한 아이템인지 아닌지를 판단하는 메서드
    /// </summary>
    /// <param name="ID">확인할 아이템의 ID</param>
    /// <returns>true : 아이템 획득, false : 아이템 미획득</returns>
    public bool GetItemState(int ID)
    {
        if (itemStateData.ContainsKey(ID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SavePlayerTransform()
    {
        if (PlayerManager.Instance.mainPlayer != null)
        {
            PlayerPrefs.SetFloat(posX, PlayerManager.Instance.mainPlayer.localPosition.x);
            PlayerPrefs.SetFloat(posY, PlayerManager.Instance.mainPlayer.localPosition.y);
            PlayerPrefs.SetFloat(posZ, PlayerManager.Instance.mainPlayer.localPosition.z);
        }

        if (PlayerManager.Instance.playerCam != null)
        {
            PlayerPrefs.SetFloat(rotX, PlayerManager.Instance.playerCam.localRotation.x);
            PlayerPrefs.SetFloat(rotY, PlayerManager.Instance.playerCam.localRotation.y);
            PlayerPrefs.SetFloat(rotZ, PlayerManager.Instance.playerCam.localRotation.z);
            PlayerPrefs.SetFloat(rotX, PlayerManager.Instance.playerCam.localRotation.w);
        }
    }
    public Vector3 NewGamePlayerPosition()
    {
        return new Vector3(newPlayerPositionX, newPlayerPositionY, newPlayerPositionZ);
    }
    public Quaternion NewGamePlayerRotation()
    {
        return new Quaternion(newPlayerRotationX, newPlayerRotationY, newPlayerRotationZ, newPlayerRotationW);
    }
    public Vector3 LoadGamePlayerPosition()
    {
        float x = PlayerPrefs.GetFloat(posX, newPlayerPositionX);
        float y = PlayerPrefs.GetFloat(posY, newPlayerPositionY);
        float z = PlayerPrefs.GetFloat(posZ, newPlayerPositionZ);

        return new Vector3(x,y,z);
    }
    public Quaternion LoadGamePlayerRotation()
    {
        float x = PlayerPrefs.GetFloat(rotY, newPlayerRotationX);
        float y = PlayerPrefs.GetFloat(rotZ, newPlayerRotationY);
        float z = PlayerPrefs.GetFloat(rotX, newPlayerRotationZ);
        float w = PlayerPrefs.GetFloat(rotW, newPlayerRotationW);

        return new Quaternion(x, y, z, w);
    }

    public bool HistoryCount()
    {
        if (gameStateData.Count.Equals(0) && itemStateData.Count.Equals(0))
        {
            gameStateData = tempGameStateData;
            itemStateData = tempItemStateData;
        }
        if (itemStateData.Count>0||gameStateData.Count>0)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 저장된 게임 데이터를 불러오는 과정을 코루틴으로 분리
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadGameSaveData_co()
    {
        // 게임상태에 대한 저장 파일이 존재하면 데이터를 읽어들임
        if (File.Exists(stateSavePath))
        {
            string stateSaveJson = null;

            yield return StartCoroutine(ReadFile_co(stateSavePath, result => stateSaveJson = result));

            if (!string.IsNullOrEmpty(stateSaveJson))
            {
                //Json 데이터를 gameStateData로 역직렬화
                gameStateData = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, bool>>>(stateSaveJson);
                tempGameStateData = gameStateData;
               
#if UNITY_EDITOR
                Debug.Log("게임 상태 로드 완료.");
#endif
            }
        }
        else
        {
            gameStateData = new Dictionary<int, Dictionary<int, bool>>();// 게임 상태 초기화
#if UNITY_EDITOR
            Debug.Log("게임 상태의 저장된 파일이 없음: 기본 상태 초기화.");
#endif
        }
    }

    /// <summary>
    /// 저장된 아이템 데이터를 불러오는 과정을 코루틴으로 분리
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadItemSaveData_co()
    {
        // 게임상태에 대한 저장 파일이 존재하면 데이터를 읽어들임
        if (File.Exists(itemSavePath))
        {
            // 아이템상태에 대한 저장 파일이 존재하면 데이터를 읽어들임
            string itemSaveJson = null;
            yield return StartCoroutine(ReadFile_co(itemSavePath, result => itemSaveJson = result));

            if (!string.IsNullOrEmpty(itemSaveJson))
            {
                itemStateData = JsonConvert.DeserializeObject<Dictionary<int, bool>>(itemSaveJson);
                tempItemStateData = itemStateData;
#if UNITY_EDITOR
                Debug.Log("아이템 획득 및 사용 내역 로드 완료.");
#endif
            }
        }
        else
        {
            itemStateData = new Dictionary<int, bool>();// 아이템 상태 초기화
#if UNITY_EDITOR
            Debug.Log("아이템의 저장된 파일이 없음: 기본 상태 초기화.");
#endif
        }
    }


    public IEnumerator ReadFile_co(string path, System.Action<string> callback)
    {
        string result = null;

        yield return new WaitUntil(() =>
        { 
        try
            {
                result = File.ReadAllText(path);
                return true;
            }
            catch
            {
                return false;
            }
        });

        callback?.Invoke(result);
    }

    public IEnumerator LoadItemData_co()
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>("Data/Json/Item_Data");
        yield return request;

        if(request.asset != null)
        {
            TextAsset itemJson = request.asset as TextAsset;
            itemData = JsonConvert.DeserializeObject<Dictionary<int, Item>>(itemJson.text);
#if UNITY_EDITOR
            Debug.Log("아이템 데이터 비동기 로드 완료!");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Item_Data.json 파일을 찾을 수 없습니다!");
#endif
        }
    }

}

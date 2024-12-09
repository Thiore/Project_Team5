using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

#if UNITY_EDITOR
public class CSVToJsonConverter : EditorWindow
{
    private string csvFilePath = "Assets/0.ETC/Data/";//csv 파일 경로
    private string csvFileName = ".csv";

    private string outputFolderPath = "Assets/Resources/Data/Json/";//저장경로
    private string jsonFileName = ".json";

    
    [MenuItem("Tools/CSV to Json Converter")]
    public static void ShowWindow()
    {
        GetWindow<CSVToJsonConverter>("CSV to Json");
    }
    private void OnGUI()
    {
        GUILayout.Label("CSV to Json Converter", EditorStyles.boldLabel);
        GUILayout.Space(20f);
        csvFilePath = EditorGUILayout.TextField("CSV File Path : ", csvFilePath);
        GUILayout.Label("CSV 파일의 경로를 설정해주세요", EditorStyles.helpBox);
        GUILayout.Space(20f);
        csvFileName = EditorGUILayout.TextField("CSV File Name : ", csvFileName);
        GUILayout.Label("CSV 파일의 이름을 설정해주세요", EditorStyles.helpBox);
        string setCSV = Path.Combine(csvFilePath, csvFileName);

        GUILayout.Space(20f);
        outputFolderPath = EditorGUILayout.TextField("Out Folder Path : ", outputFolderPath);
        GUILayout.Label("Json 파일을 저장할 경로를 설정해주세요", EditorStyles.helpBox);
        GUILayout.Space(20f);
        jsonFileName = EditorGUILayout.TextField("Json File Name : ", jsonFileName);
        GUILayout.Label("Json 파일의 이름을 설정해주세요", EditorStyles.helpBox);

        if (GUILayout.Button("Convert"))
        {
            ConvertCSVToJson(setCSV, outputFolderPath,jsonFileName);
        }
    }

    private void ConvertCSVToJson(string csvPath, string outputPath, string jsonName)
    {
        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV file not found at: " + csvPath);
            return;
        }

        // CSV파일의 모든 줄 읽기
        string[] lines = File.ReadAllLines(csvPath);
        //배열의 크기가 1보다 작다면 담겨있는 내용이 없다는 뜻이므로 오류메세지 호출 후 return
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file has no data.");
            return;
        }

        // 첫 번째 줄에서 헤더 추출
        string[] headers = lines[0].Split(',');

        // 데이터를 저장할 Dictionary 생성
        Dictionary<int, Dictionary<string, string>> dataDic = new Dictionary<int, Dictionary<string, string>>();

        //  헤더를 제외한 각 줄을 처리
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
            {
                data[headers[j]] = values[j];
            }

            dataDic.Add(i-1,data); //0부터 시작하도록 조정
        }

        // Convert the list to JSON format
        string json = JsonConvert.SerializeObject(dataDic, Formatting.Indented);

        // 출력 폴더가 존재하지 않으면 생성
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // JSON 파일로 저장
        string jsonFilePath = Path.Combine(outputPath, jsonName);
        File.WriteAllText(jsonFilePath, json);

        AssetDatabase.Refresh();
        Debug.Log("JSON file saved to: " + jsonFilePath);
    }

}

#endif
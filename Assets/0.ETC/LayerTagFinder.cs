using UnityEngine;
using UnityEditor;

public class LayerTagFinder : EditorWindow
{
    private string searchTag = "Untagged";
    private int searchLayer = 2;

    [MenuItem("Tools/Layer and Tag Finder")]
    static void Init()
    {
        LayerTagFinder window = (LayerTagFinder)EditorWindow.GetWindow(typeof(LayerTagFinder));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Search for Objects by Tag or Layer", EditorStyles.boldLabel);

        // 태그 입력
        searchTag = EditorGUILayout.TagField("Search Tag", searchTag);

        // 레이어 입력
        searchLayer = EditorGUILayout.LayerField("Search Layer", searchLayer);

        if (GUILayout.Button("Find Objects"))
        {
            FindObjects();
        }
    }

    void FindObjects()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Hierarchy에 존재하는 오브젝트만 필터링
            if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                continue;

            // 태그 매칭
            if (!string.IsNullOrEmpty(searchTag) && obj.tag == searchTag)
            {
                Debug.Log($"[Tag] Found Object: {obj.name}", obj);
            }

            // 레이어 매칭
            if (obj.layer == searchLayer)
            {
                Debug.Log($"[Layer] Found Object: {obj.name}", obj);
            }
        }

        Debug.Log("Search Complete!");
    }
}

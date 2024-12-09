using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private Button loadGameBtn;
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button settingBtn;

    private void OnEnable()
    {
        settingBtn.onClick.AddListener(SettingsManager.Instance.OnSettingPage);
        newGameBtn.onClick.AddListener(GameManager.Instance.NewGame);

        loadGameBtn.gameObject.SetActive(DataSaveManager.Instance.HistoryCount());
        if(loadGameBtn.gameObject.activeSelf)
        {
            loadGameBtn.onClick.AddListener(GameManager.Instance.LoadGame);
        }    
    }
    private void OnDisable()
    {
        settingBtn.onClick.RemoveListener(SettingsManager.Instance.OnSettingPage);
        newGameBtn.onClick.RemoveListener(GameManager.Instance.NewGame);
        if (loadGameBtn.gameObject.activeSelf)
        {
            loadGameBtn.onClick.RemoveListener(GameManager.Instance.LoadGame);
        }
    }

}

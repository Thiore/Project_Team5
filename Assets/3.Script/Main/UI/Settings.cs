using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void LoadLobby()
    {
        SaveManager.Instance.SaveGameState();
        SceneManager.LoadScene("Lobby");
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}

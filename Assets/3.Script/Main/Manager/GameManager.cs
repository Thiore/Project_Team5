using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager instance = null;
    public static GameManager Instance { get; private set; }

    public bool isInput;

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
    }


}

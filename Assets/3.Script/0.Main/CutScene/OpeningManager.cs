using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile openingVolume;
    [SerializeField] private VolumeProfile mainGameVolume;

    private void OnEnable()
    {
        if (GameManager.Instance.gameType.Equals(eGameType.LoadGame))
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        globalVolume.profile = mainGameVolume;
    }
}

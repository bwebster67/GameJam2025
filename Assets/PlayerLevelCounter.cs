using TMPro;
using UnityEngine;
using System;

public class PlayerLevelCounter : MonoBehaviour
{
    public TMP_Text text;
    public int playerLevel = 0;

    private void OnEnable()
    {
        PlayerMusicController.LevelUpEvent += AddLevel;
    }

    private void OnDisable()
    {
        PlayerMusicController.LevelUpEvent -= AddLevel;
    }

    public event Action LevelUpEvent;

    public void AddLevel()
    {
        playerLevel++;
        text.text = $"{playerLevel}";
    }

}

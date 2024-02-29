using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats; // 0: health, 1: stamina

    public float[] playerPositionAndRotation; //position x, y, z, rotation x, y, z

    public string[] inventoryContent;

    public string[] quickSlotContent;

    public PlayerData(float[] _playerStats, float[] _playerPositionAndRotation, string[] _inventoryContent, string[] _quickSlotContent)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPositionAndRotation;
        inventoryContent = _inventoryContent;
        quickSlotContent = _quickSlotContent;
    }
}

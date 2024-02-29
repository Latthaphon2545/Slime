using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats; // 0: health, 1: stamina

    public float[] playerPositionAndRotation; //position x, y, z, rotation x, y, z

    //public string[] inventoryContent;

    public PlayerData(float[] _playerStats, float[] _playerPositionAndRotation)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPositionAndRotation;
    }
}

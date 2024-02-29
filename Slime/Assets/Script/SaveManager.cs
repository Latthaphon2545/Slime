using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public bool isSavingJson;

    #region || --General Section-- ||

    public void SaveGame()
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();

        SaveAllGameData(data);
    }

    private PlayerData GetPlayerData()
    {

        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentStamina;

        float[] playerPosition = new float[6];
        playerPosition[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosition[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosition[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosition[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosition[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosition[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        return new PlayerData(playerStats, playerPosition);
    }

    public void SaveAllGameData(AllGameData gameData)
    {
        if (isSavingJson)
        {
            // saveGameDataToJsonFile(gameData);
        }
        else
        {
            saveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region || -- To Binary Section-- ||

    public void saveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatBinary = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_Game1";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatBinary.Serialize(stream, gameData);
        stream.Close();

        print("Data save at" + path);
    }

    public AllGameData loadGAmeDataFromBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_Game1";

        if (File.Exists(path))
        {
            BinaryFormatter formatBinary = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            AllGameData data = formatBinary.Deserialize(stream) as AllGameData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region || -- Setting Section -- ||

    #region || --Volumn Setting-- ||
    // music
    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public float LoadMusicVolume(float volume)
    {
        return PlayerPrefs.GetFloat("MusicVolume");
    }

    // FX
    public void SaveFxVolume(float volume)
    {
        PlayerPrefs.SetFloat("FxVolume", volume);
        PlayerPrefs.Save();
    }

    public float LoadFxcVolume(float volume)
    {
        return PlayerPrefs.GetFloat("FxVolume");
    }


    [System.Serializable]
    public class VolumeSetting
    {
        public float music;
        public float fx;
        public float master;
    }

    public void SaveVolumeSetting(float _music, float _fx, float _master)
    {
        VolumeSetting volumeSetting = new VolumeSetting()
        {
            music = _music,
            fx = _fx,
            master = _master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSetting));
        PlayerPrefs.Save();

        print("Save Data");
    }


    public VolumeSetting LoadvolumeSetting1()
    {
        return JsonUtility.FromJson<VolumeSetting>(PlayerPrefs.GetString("Volume")); // return ที่เราบันทึกค้าของเสียงไว้
    }

    #endregion

    #endregion

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    #region || --Saving-- ||

    public void SaveGame()
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();

        SavingTypeSwitch(data);
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

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavingJson)
        {
            // saveGameDataToJsonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region || --Loading-- ||

    public AllGameData LoadindTypeSwitch()
    {
        if (isSavingJson)
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(); // ใช้ไปก่อน
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        // Player Data
        SetPlayerData(LoadindTypeSwitch().playerData);

        // Enviroment Data

    }

    private void SetPlayerData(PlayerData playerData)
    {
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentStamina = playerData.playerStats[1];

        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;
        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
    }

    public void StartLoadGame()
    {
        SceneManager.LoadScene("Game");
        StartCoroutine(DelayLaoding()); ;
    }

    private IEnumerator DelayLaoding()
    {
        yield return new WaitForSeconds(1f);

        LoadGame();

    }


    #endregion

    #endregion

    #region || -- To Binary Section-- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatBinary = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save_Game1";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatBinary.Serialize(stream, gameData);
            print("Data saved at " + path);
        }
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_Game1";

        if (File.Exists(path))
        {
            BinaryFormatter formatBinary = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if (stream.Length != 0)
            {
                AllGameData data = formatBinary.Deserialize(stream) as AllGameData;
                stream.Close();

                print("Data loaded from " + path);

                return data;
            }
            else
            {
                print("File is empty");
                stream.Close();
                return null;
            }
        }
        else
        {
            print("File not found");
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

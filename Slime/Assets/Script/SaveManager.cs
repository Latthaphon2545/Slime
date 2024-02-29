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

    string jsonPathProject;
    string jsonPathPersistent;
    string binaryPath;

    string fileName = "save_Game1";

    public bool isLoading;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistent = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }


    public bool isSavingJson;

    #region || --General Section-- ||

    #region || --Saving-- ||

    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        data.eniromentData = GetEniromentData();

        SavingTypeSwitch(data, slotNumber);
    }

    private EniromentData GetEniromentData()
    {
        List<string> itemsPickedUp = InventorySystem.Instance.itemsPickedup;

        return new EniromentData(itemsPickedUp);
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

        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlot = GetQuickSlotContent();

        return new PlayerData(playerStats, playerPosition, inventory, quickSlot);
    }

    private string[] GetQuickSlotContent()
    {
        List<string> temp = new List<string>();

        foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if(slot.transform.childCount != 0)
            {
                string itemName = slot.transform.GetChild(0).name;
                string str = "(Clone)";
                string result = itemName.Replace(str, "");
                temp.Add(result);
            }
        }

        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavingJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }

    #endregion

    #region || --Loading-- ||

    public AllGameData LoadindTypeSwitch(int slotNumber)
    {
        if (isSavingJson)
        {
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        // Player Data
        SetPlayerData(LoadindTypeSwitch(slotNumber).playerData);

        // Enviroment Data
        SetEnviromentData((LoadindTypeSwitch(slotNumber).eniromentData));

        isLoading = false;

    }

    private void SetEnviromentData(EniromentData environmentData)
    {
        foreach(Transform itemType in EnviromentManager.Instance.allItem.transform)
        {
            foreach(Transform item in itemType.transform)
            {
                if(environmentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemsPickedup = environmentData.pickedupItems;
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


        // inventory
        foreach (string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddToInventory(item);
        }

        // quick slot
        foreach (string item in playerData.quickSlotContent)
        {
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();

            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }
    }

    public void StartLoadGame(int slotNumber)
    {
        isLoading = true;
        SceneManager.LoadScene("Game");
        StartCoroutine(DelayLaoding(slotNumber)); ;
    }

    private IEnumerator DelayLaoding(int slotNumber)
    {
        yield return new WaitForSeconds(1f);

        LoadGame(slotNumber);

    }


    #endregion

    #endregion

    #region || -- To Binary Section-- ||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatBinary = new BinaryFormatter();

        string path = binaryPath + fileName + slotNumber + ".bin";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatBinary.Serialize(stream, gameData);
            print("Data saved at " + path);
        }
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {
        string path = binaryPath + fileName + slotNumber + ".bin";

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

    #region || -- To Json Section-- ||

    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        // string encrypted = EncryptionDecryption(json);

        string path = jsonPathProject + fileName + slotNumber + ".json";

        using (StreamWriter stream = new StreamWriter(path))
        {
            stream.Write(json);
            print("Data saved at " + path);
        }
    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        string path = jsonPathProject + fileName + slotNumber + ".json";

        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();

            // string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            print("Data loaded from " + path);

            return data;
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

    #region || -- Encryption -- ||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567890";
        string result = string.Empty;
        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }


    #endregion

    #region || -- Utility -- ||

    public bool DoseFileExist(int slotNumber)
    {
        if (isSavingJson)
        {
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isSlotEmpty(int slotNumber)
    {
        if (DoseFileExist(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }


    #endregion
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSaveManager : MonoBehaviour
{
    public static MainMenuSaveManager Instance { get; set; }

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
    }



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




}

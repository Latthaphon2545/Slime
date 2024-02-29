using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; set; }


    public Button backBTN;

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider fxSlider;
    public GameObject fxValue;

    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
            MainMenuSaveManager.Instance.SaveVolumeSetting(musicSlider.value, fxSlider.value, masterSlider.value);
        });

        StartCoroutine(LoadAndApplySetting());
    }

    private IEnumerator LoadAndApplySetting()
    {
        LoadandSetVolume();

        // ในนี้สามารถโหลดอย่างอื่นที่เราบันทึกไว้ได้

        yield return new WaitForSeconds(0.1f);
    }

    private void LoadandSetVolume()
    {
        MainMenuSaveManager.VolumeSetting volumeSetting = MainMenuSaveManager.Instance.LoadvolumeSetting1();
        masterSlider.value = volumeSetting.master;
        musicSlider.value = volumeSetting.music;
        fxSlider.value = volumeSetting.fx;

        print("Load and Apply");
    }



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



    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        fxValue.GetComponent<TextMeshProUGUI>().text = "" + (fxSlider.value) + "";
    }


}

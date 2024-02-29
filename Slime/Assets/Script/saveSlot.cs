using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class saveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    public GameObject alertUI;
    Button yesBTN;
    Button noBTN;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("yesBTN").GetComponent<Button>();
        noBTN = alertUI.transform.Find("noBTN").GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.isSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverwrite();
            }
        });
    }

    private void Update()
    {
        if (SaveManager.Instance.isSlotEmpty(slotNumber))
        {
            buttonText.fontSize = 15;
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.fontSize = 8;
            buttonText.text = PlayerPrefs.GetString("slot" + slotNumber + "Description");
        }
    }

    public void DisplayOverwrite()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(() =>
        {
            SaveGameConfirmed();
            alertUI.SetActive(false);
        });

        noBTN.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    private void SaveGameConfirmed()
    {
        SaveManager.Instance.SaveGame(slotNumber);

        DateTime dateTime = DateTime.Now;
        string date = dateTime.ToString("dd/MM/yyyy HH:mm");

        buttonText.fontSize = 8;
        string description = "Save Game " + slotNumber + "\n" + date;
        buttonText.text = description;

        PlayerPrefs.SetString("slot" + slotNumber + "Description", description);

        SaveManager.Instance.DeselectButton();
    }
}

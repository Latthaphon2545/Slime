using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (SaveManager.Instance.isSlotEmpty(slotNumber))
        {
            buttonText.text = "";
        }
        else
        {
            buttonText.fontSize = 8;
            buttonText.text = PlayerPrefs.GetString("slot" + slotNumber + "Description");
        }
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.isSlotEmpty(slotNumber) == false)
            {
                SaveManager.Instance.StartLoadGame(slotNumber);
                SaveManager.Instance.DeselectButton();
            }
            else
            {

            }
        });
    }

}

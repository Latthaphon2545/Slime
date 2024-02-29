using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numberHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selectedItemModel;


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


    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelecQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelecQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelecQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelecQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelecQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelecQuickSlot(6);
        }
    }

    void SelecQuickSlot(int numberPress)
    {
        if (CheckIfSlotIsFull(numberPress) == true)
        {
            if (selectedNumber != numberPress)
            {
                selectedNumber = numberPress;

                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelecedItem(numberPress);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;


                SetEquippedModel(selectedItem);

                foreach (Transform child in numberHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.yellow;
                }

                Text toBeChange = numberHolder.transform.Find("number" + numberPress).transform.Find("Text").GetComponent<Text>();
                toBeChange.color = Color.white;

                Debug.Log("เลือกแล้ว: " + selectedItem);

            }
            else // Try to select same slot
            {
                selectedNumber -= 1; // null

                if (selectedItem != null)
                {
                    selectedItem.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel,gameObject);
                    selectedItemModel = null;
                }

                foreach (Transform child in numberHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.yellow;
                }

                Debug.Log("ยกเลิกล่ะ: " + selectedItem);
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel, gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
            new Vector3(-0.44f, 0.51f, 1.04f), Quaternion.Euler(25.8f, -54.9f, -29f));

        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }



    GameObject GetSelecedItem(int numberPress)
    {
        return quickSlotsList[numberPress - 1].transform.GetChild(0).gameObject;
    }



    bool CheckIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }



    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        InventorySystem.Instance.ReCalculeList();

    }




    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }



    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter++;
            }
        }

        if (counter == quickSlotsList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsHoldingWeapon()
    {
        Debug.Log("selectedItem: " + selectedItem);
        if (selectedItem != null)
       {
            Weapon weaponComponent = selectedItem.GetComponent<Weapon>();
            Debug.Log("weaponComponent: " + weaponComponent);

            if (weaponComponent != null)
            {
                Debug.Log("ถืออยู่");
                return true;
            }
            else
            {
                Debug.Log("บ่ถืออยู่");
                return false;
            }
        }
       else
       {
            Debug.Log("บ่ถืออยู่");
            return false;
       }
    }

    public int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else
        {
            return 0;
        }
    }

}
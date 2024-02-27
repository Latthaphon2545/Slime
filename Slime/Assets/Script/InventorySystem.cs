using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public GameObject ItemInfoUi;

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public GameObject bagUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    public GameObject pickupAlert;
    public TMP_Text pickupName;
    public Image pickupImg;
    private Coroutine pickupAlertCoroutine;

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


    void Start()
    {
        isOpen = false;

        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach(Transform child in bagUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            inventoryScreenUI.SetActive(true);
            isOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isOpen = false;

            SelectionManager.Instance.EndableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    public void AddToInventory(String itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        Sprite sprite = itemToAdd.GetComponent<Image>().sprite;
        TriggerPickupPopUp(itemName, sprite);


        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSpriteImg)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImg.sprite = itemSpriteImg;

        if (pickupAlertCoroutine != null)
        {
            StopCoroutine(pickupAlertCoroutine);
        }

        pickupAlertCoroutine = StartCoroutine(HidePickupAlertAfterDelay(1.5f));
    }

    IEnumerator HidePickupAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        pickupAlert.SetActive(false);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }

        }
        return new GameObject();
    }

    public bool checkIsFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter++;
            }
        }


        if(counter == slotList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter !=0 )
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter--;
                }
            }
        }

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    public void ReCalculeList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str = "(Clone)";
                string result = name.Replace(str,"");

                itemList.Add(result);
            }
        }
    }

}
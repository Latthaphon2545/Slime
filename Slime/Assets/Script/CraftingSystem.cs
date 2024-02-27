using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject detailScreenUI;

    public Text nameItem;

    public List<string> inventoryItemList = new List<string>();


    Button toolBTN;

    Button craftBTN;

    Text itemReq;

    public bool isOpen;

    public BluePrint AxeBLP = new BluePrint("Axe", 2, new List<string> { "Apple", "Carrot" }, new List<int> { 3, 3 });


    public static CraftingSystem Instance { get; set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        toolBTN = craftingScreenUI.transform.Find("ToolsButton").transform.Find("Icon").GetComponent<Button>();
        toolBTN.onClick.AddListener(delegate{OpenToolsCategory();});

        itemReq = craftingScreenUI.transform.Find("ToolsButton").transform.Find("itemReq").GetComponent<Text>();
        craftBTN = craftingScreenUI.transform.Find("ToolsButton").transform.Find("buttonCraft").GetComponent<Button>();

        craftBTN.onClick.AddListener(delegate { craftAnyItem(AxeBLP); });

        nameItem.text = AxeBLP.itemName;


    }

    private void craftAnyItem(BluePrint blueprintTocraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintTocraft.itemName);

        for (var i = 0; i < blueprintTocraft.numOfRequirements; i++)
        {
            InventorySystem.Instance.RemoveItem(blueprintTocraft.itemReq[i], blueprintTocraft.itemreqAmount[i]);
            Debug.Log(blueprintTocraft.itemReq[i]);
            Debug.Log(blueprintTocraft.itemreqAmount[i]);
        }

        StartCoroutine(Calculate());
    }

    void OpenToolsCategory()
    {
        detailScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            detailScreenUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RefreshNeededItems()
    {
        int apple_count = 0;
        int carrot_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch(itemName)
            {
                case "Apple":
                    Debug.Log("Pick up Apple");
                    apple_count++;
                    break;

                case "Carrot":
                    Debug.Log("Pick up Carrot");
                    carrot_count++;
                    break;
            }
        }

        itemReq.text = "3 Apple ["+ apple_count + "] \n3 Stick [" + carrot_count + "]";

        if(apple_count >=3 && carrot_count >=3)
        {
            craftBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBTN.gameObject.SetActive(false);
        }
    }

    public IEnumerator Calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();

        RefreshNeededItems();
    }
}

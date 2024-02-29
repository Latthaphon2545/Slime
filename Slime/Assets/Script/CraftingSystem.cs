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

    public Button toolUIBTN, SurvivlaUIBTN, RefindUIBTN;
    public GameObject toolUI, SurvivlaUI, RefindUI;

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
        itemReq = craftingScreenUI.transform.Find("ToolsButton").transform.Find("itemReq").GetComponent<Text>();
        craftBTN = craftingScreenUI.transform.Find("ToolsButton").transform.Find("buttonCraft").GetComponent<Button>();

        craftBTN.onClick.AddListener(delegate { craftAnyItem(AxeBLP); });

        toolUIBTN.onClick.AddListener(openToolsUi);
        SurvivlaUIBTN.onClick.AddListener(openSurvivlaUi);
        RefindUIBTN.onClick.AddListener(openRefindUi);

        nameItem.text = AxeBLP.itemName;


    }

    private void openRefindUi()
    {
        //toolUI.SetActive(false);
        //RefindUI.SetActive(true);
        //SurvivlaUI.SetActive(false);
        //Debug.Log("Eiei");
    }

    private void openSurvivlaUi()
    {
        //toolUI.SetActive(false);
        //RefindUI.SetActive(false);
        //SurvivlaUI.SetActive(true);
    }

    private void openToolsUi()
    {
        toolUI.SetActive(true);
        RefindUI.SetActive(false);
        SurvivlaUI.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            craftingScreenUI.SetActive(true);
            isOpen = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else if ((Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Escape)) && isOpen)
        {
            craftingScreenUI.SetActive(false);

            if(!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EndableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            isOpen = false;
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

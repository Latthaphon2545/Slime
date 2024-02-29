using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameMenuManager : MonoBehaviour
{
    public static inGameMenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject menu;

    public bool isMenuIsOpen;

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
        if(Input.GetKeyDown(KeyCode.M)  && !isMenuIsOpen)
        {
            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);

            isMenuIsOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.M) && isMenuIsOpen)
        {

            saveMenu.SetActive(false);
            settingMenu.SetActive(false);
            menu.SetActive(true);

            uiCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuIsOpen = false;

            if(CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EndableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    public void TempSaveGame()
    {
        SaveManager.Instance.SaveGame();
    }

}

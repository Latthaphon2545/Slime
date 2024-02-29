using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Button LoadGameBTN;

    private void Start()
    {
        if (LoadGameBTN != null)
        {
            LoadGameBTN.onClick.AddListener(() =>
            {
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.StartLoadGame();
                }
                else
                {
                    Debug.LogError("SaveManager.Instance is null");
                }
            });
        }
        else
        {
            Debug.LogError("LoadGameBTN is null");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

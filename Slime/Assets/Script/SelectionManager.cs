using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject interaction_Info_UI;
    private Text interaction_text;

    public GameObject selectedObject;

    public Image centerDotIcon;
    public Image centerHandIcon;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject Interactable = selectionTransform.GetComponent<InteractableObject>();

            if (Interactable && Interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = Interactable.gameObject;

                interaction_text.text = Interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                if (Interactable.CompareTag("pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    centerHandIcon.gameObject.SetActive(true);
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    centerHandIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                onTarget = false;

                interaction_Info_UI.SetActive(false);

                centerDotIcon.gameObject.SetActive(true);
                centerHandIcon.gameObject.SetActive(false);
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);

            centerDotIcon.gameObject.SetActive(true);
            centerHandIcon.gameObject.SetActive(false);
        }
    }

    public void DisableSelection()
    {
        centerHandIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    internal void EndableSelection()
    {
        centerHandIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}

using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject interaction_Info_UI;
    private TMP_Text interaction_text;

    public GameObject selectedObject;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
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
            }
            else
            {
                onTarget = false;

                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }
}

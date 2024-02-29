using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject interaction_Info_UI;
    private Text interaction_text;

    public GameObject selectedObject;

    public Image centerDotIcon;
    public Image centerHandIcon;

    public bool handVisible;

    public GameObject selectedTree;
    public GameObject chopHloder;

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


            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();


            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHloder.gameObject.SetActive(true);
            }
            else
            {
                if(selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHloder.gameObject.SetActive(true);
                }

                chopHloder.gameObject.SetActive(false);
            }



            if (Interactable && Interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = Interactable.gameObject;

                interaction_text.text = Interactable.GetItemName() + "(E)";
                interaction_Info_UI.SetActive(true);
                if (Interactable.CompareTag("pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    centerHandIcon.gameObject.SetActive(true);

                    handVisible = true;
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    centerHandIcon.gameObject.SetActive(false);

                    handVisible = false;
                }
            }
            else
            {
                onTarget = false;

                interaction_Info_UI.SetActive(false);

                centerDotIcon.gameObject.SetActive(true);
                centerHandIcon.gameObject.SetActive(false);

                handVisible = false;
            }


            // Monster
            Monster monster = selectionTransform.GetComponent<Monster>();

            if(monster && monster.playerInRange)
            {
                interaction_text.text = monster.monsterName + "(E)";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(DealDamage(monster, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
                else
                {
                    interaction_text.text = "";
                    interaction_Info_UI.SetActive(false);
                }
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);

            centerDotIcon.gameObject.SetActive(true);
            centerHandIcon.gameObject.SetActive(false);

            handVisible = false;
        }
    }

    IEnumerator DealDamage(Monster monster, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        monster.TakeDamage(damage);
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

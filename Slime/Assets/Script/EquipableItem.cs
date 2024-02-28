using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.Instance.isOpen == false &&
            SelectionManager.Instance.handVisible == false) // เมาส์ซ้าย
        {

            GameObject selectedTree = SelectionManager.Instance.selectedTree;

            if (selectedTree != null)
            {
                SoundManager.Instance.playSound(SoundManager.Instance.chopSound);
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }

            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
        }
    }


    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.playSound(SoundManager.Instance.toolSwingSound);
    }
}

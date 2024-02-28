using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float maxTreeHealth;
    public float currentTreeHealth;

    public Animator animator;


    private void Start()
    {
        currentTreeHealth = maxTreeHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetHit()
    {
        StartCoroutine(hit());
    }

    public IEnumerator hit()
    {
        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("shake");

        if(currentTreeHealth <= 0)
        {
            TreeIsDead();
        }

        currentTreeHealth -= 1;
    }

    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHloder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Stump"),
            new Vector3(treePosition.x, treePosition.y+1.5f, treePosition.z), Quaternion.Euler(30, 30, 30));
    }

    private void Update()
    {
        if(canBeChopped)
        {
            GlobalState.Instance.resourceHealth = currentTreeHealth;
            GlobalState.Instance.resourceMaxHealth = maxTreeHealth;
        }
    }

}

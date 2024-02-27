using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler
{

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                Debug.Log("Have Item");
                return transform.GetChild(0).gameObject;
            }
            Debug.Log("Not have Item");
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        //if there is not item already then set our item.
        if (!Item)
        {

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

        }


    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EniromentData
{
    public List<string> pickedupItems;

    public EniromentData(List<string> _pickedupItems)
    {
        pickedupItems = _pickedupItems;
    }
}
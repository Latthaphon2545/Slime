using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint
{
    public string itemName;
    public List<string> itemReq;
    public List<int> itemreqAmount;
    public int numOfRequirements;

    public BluePrint(string name, int reqNum, List<string> ReqStr, List<int> ReqInt)
    {
        itemName = name;
        numOfRequirements = reqNum;
        itemReq = new List<string>(ReqStr);
        itemreqAmount = new List<int>(ReqInt);
    }
}

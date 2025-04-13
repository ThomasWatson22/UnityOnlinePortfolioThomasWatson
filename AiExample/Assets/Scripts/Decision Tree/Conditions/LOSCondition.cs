using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSCondition : ConditionNode
{
    public bool hasLOS { get; set; }
    public LOSCondition()
    {
        name = "LOS Condition";
        hasLOS = false;
    }

    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return hasLOS;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateCondition : ConditionNode
{
    public bool hasToChange { get; set; }
    public ChangeStateCondition()
    {
        name = "LOS Condition";
        hasToChange = false;
    }
    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return hasToChange;
    }
}

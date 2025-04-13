using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPatrolCondition : ConditionNode
{
    public bool canPatrol { get; set; }
    public MoveToPatrolCondition()
    {
        name = "LOS Condition";
        canPatrol = false;
    }

    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return canPatrol;
    }
}
